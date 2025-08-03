using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableManager : MonoBehaviour, IInit
{
    public static TableManager Instance { get; private set; }
    
    [Header("Rail")]
    [SerializeField] private Sprite[] railSprites;
    [SerializeField] private GameObject railPref, railParent;
    
    [Header("Serving")]
    [SerializeField] private GameObject dishPref, dishParent;

    [Header("Menu")]
    [SerializeField] private GameObject menuPref;
    [SerializeField] private GameObject menuParent;
    
    [HideInInspector]
    public int RailCnt { get; private set; }
    public int DishCnt { get; private set; }
    
    public Dictionary<int, DishBehaviour> DishBehaviourDict { get; private set; }

    private readonly float _railMaxPosY = 1.35f;
    public float ServingMinPosX { get; private set; }
    private float _rotationMinPosX, _railMinPosX;

    private int currCompletedRotCnt;
    private List<int> _checkDishIdx;

    public List<Sprite> dishSprites, sushiSprites;
    
    private DishBehaviour _selectedDish;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    #region Initialization
    public void Init()
    {
        RailCnt = (int)MainSceneManager.Instance.CurrStageData["RailCnt"];
        if (RailCnt < 1) { Debug.LogError($"something's wrong [railCnt == {RailCnt}]"); return; }
        
        currCompletedRotCnt = 0;
        _checkDishIdx = new List<int>();
        DishBehaviourDict = new Dictionary<int, DishBehaviour>();
        
        _selectedDish = null;
        
        GenerateRail();
        GenerateDish();
        GenerateMenu();
    }

    private void GenerateRail()
    {
        foreach (Transform rail in railParent.transform)
            Destroy(rail.gameObject);
        
        float currPosX = -1f * (RailCnt / 2) * MainSceneManager.Instance.PosXFactor;
        currPosX -= RailCnt % 2 == 1 ? MainSceneManager.Instance.PosXFactor : 0.5f * MainSceneManager.Instance.PosXFactor;
        
        _rotationMinPosX = currPosX;
        _railMinPosX = currPosX + MainSceneManager.Instance.PosXFactor;

        GameObject tmpObj;
        
        for (int i = -1; i < RailCnt+1; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (j == 0)
                {
                    if (i == -1 || i == RailCnt)
                    {
                        tmpObj = Instantiate(railPref, railParent.transform);
                        tmpObj.transform.localPosition = new Vector3(currPosX, 0, 0);
                        tmpObj.GetComponent<SpriteRenderer>().sprite = railSprites[2];
                    }
                }
                else
                {
                    tmpObj = Instantiate(railPref, railParent.transform);
                    tmpObj.transform.localPosition = new Vector3(currPosX, _railMaxPosY * j, 0);
                    if (i == -1 || i == RailCnt)
                    {
                        SpriteRenderer spr = tmpObj.GetComponent<SpriteRenderer>();
                        spr.sprite = railSprites[1];
                        spr.flipX = i == RailCnt;
                        spr.flipY = j == -1;
                    }
                    else
                    {
                        tmpObj.GetComponent<SpriteRenderer>().sprite = railSprites[0];
                    }
                }
            }
            currPosX += MainSceneManager.Instance.PosXFactor;
        }
    }

    private void GenerateDish()
    {
        string[] dishColor = CSVReader.ParseDollar((string)MainSceneManager.Instance.CurrStageData["DishColor"]);
        DishCnt = dishColor.Length;
        if (DishCnt > RailCnt) { Debug.LogError($"DishCnt is bigger than railCnt [railCnt == {RailCnt}, DishCnt == {DishCnt}]"); return; }
        if (DishCnt < 1) { Debug.LogError($"something's wrong [DishCnt == {DishCnt}]"); return; }
        
        foreach (Transform dish in dishParent.transform)
            Destroy(dish.gameObject);
        
        float currPosX = -1f * (DishCnt / 2) * MainSceneManager.Instance.PosXFactor;
        currPosX += RailCnt % 2 == 1 ? 0f : 0.5f * MainSceneManager.Instance.PosXFactor;
        ServingMinPosX = currPosX;
        
        for (int i = 0; i < DishCnt; i++)
        {
            GameObject tmpObj = Instantiate(dishPref, dishParent.transform);
            DishBehaviourDict.Add(i, tmpObj.GetComponent<DishBehaviour>());
            if (Enum.TryParse(dishColor[i], ignoreCase: true, out ColorTypes color))
            {
                DishBehaviourDict[i].InitDish(SushiTypes.SushiEmpty, color, new Vector3(currPosX, _railMaxPosY * -1f, 0));
                currPosX += MainSceneManager.Instance.PosXFactor;
            }
            else Debug.LogError("Color Error: " + dishColor[i]);
        }
    }
    
    private void GenerateMenu()
    {
        string[] menuSushi = CSVReader.ParseDollar((string)MainSceneManager.Instance.CurrStageData["Menu"]);
        if (menuSushi.Length < 1) { Debug.LogError($"something's wrong [menuSushi.Length == {menuSushi.Length}]"); return; }
        
        foreach (Transform menu in menuParent.transform)
            Destroy(menu.gameObject);
        
        for (int i = 0; i < menuSushi.Length; i++)
        {
            var menuBehaviour = Instantiate(menuPref, menuParent.transform).GetComponent<MenuBehaviour>();
            if (Enum.TryParse(menuSushi[i], ignoreCase: true, out SushiTypes sushi)) menuBehaviour.InitMenu(sushi);
            else Debug.LogError("Menu Sushi Error: " + menuSushi[i]);
        }
    }

    
    #endregion

    public void DishSelected(DishBehaviour dishBehaviour)
    {
        if (MainSceneManager.Instance.CookStarted)
        {
            
        }
        else
        {
            if (_selectedDish != null)
            {
                Vector3 firstCurrPos = _selectedDish.DishData.CurrPos, secondCurrPos = dishBehaviour.DishData.CurrPos;
                _selectedDish.SwapPosition(secondCurrPos);
                dishBehaviour.SwapPosition(firstCurrPos);
                
                int firstDishIdx = 0, secondDishIdx = 0;
                foreach (var pair in DishBehaviourDict)
                {
                    if (pair.Value == _selectedDish)
                        firstDishIdx = pair.Key;
                    else if (pair.Value == dishBehaviour)
                        secondDishIdx = pair.Key;
                }

                DishBehaviourDict[firstDishIdx] = dishBehaviour;
                DishBehaviourDict[secondDishIdx] = _selectedDish;
                
                _selectedDish = null;
            }
            else _selectedDish = dishBehaviour;
        }
    }

    public void DishDeSelected()
    {
        _selectedDish = null;
    }

    public void ReadyToCook()
    {
        if (_selectedDish == null) return;
        _selectedDish.ReadyToCook();
        _selectedDish = null;
    }
    
    public void RotateDishOnce()
    {
        InteractionManager.Instance.InitCatDishRelative();
        
        currCompletedRotCnt = 0;
        _checkDishIdx = new List<int>();
        
        for (int i = 0; i < DishBehaviourDict.Count; i++)
        {
            DishBehaviour dish = DishBehaviourDict[i];
            Vector3 dishEndPos = Vector3.zero, dishStartPos = dish.DishData.CurrPos;
            bool moveXFirst = false;
            
            if (Mathf.Abs(dishStartPos.x - _railMinPosX) <= 0.0001f && dishStartPos.y < 0f)
            {
                // 좌하단 -> 좌
                dishEndPos.x = _rotationMinPosX; dishEndPos.y = 0; moveXFirst = true;
            }
            else if (Mathf.Abs(dishStartPos.x - _rotationMinPosX) <= 0.0001f)
            {
                // 좌 -> 좌상단
                dishEndPos.x = _railMinPosX; dishEndPos.y = _railMaxPosY; moveXFirst = false;
            }
            else if (Mathf.Abs(dishStartPos.x + _railMinPosX) <= 0.0001f && dishStartPos.y > 0f)
            {
                // 우상단 -> 우
                dishEndPos.x = -1f * _rotationMinPosX; dishEndPos.y = 0; moveXFirst = true;
            }
            else if (Mathf.Abs(dishStartPos.x + _rotationMinPosX) <= 0.0001f)
            {
                // 우 -> 우하단
                dishEndPos.x = -1f * _railMinPosX; dishEndPos.y = -1f * _railMaxPosY; moveXFirst = false;
            }
            else if (dishStartPos.y < 0f)
            {
                // 밑줄
                dishEndPos.x = dishStartPos.x - MainSceneManager.Instance.PosXFactor; dishEndPos.y = dishStartPos.y;
            }
            else
            {
                // 윗줄
                dishEndPos.x = dishStartPos.x + MainSceneManager.Instance.PosXFactor; dishEndPos.y = dishStartPos.y;
            }

            if (dishEndPos.y > 0.0001f
                && dishEndPos.x - DiningManager.Instance.DiningMinPosX >= -0.0001f
                && dishEndPos.x - (DiningManager.Instance.DiningMinPosX +
                                             MainSceneManager.Instance.PosXFactor * (DiningManager.Instance.CatCnt - 1)) <= 0.0001f)
                _checkDishIdx.Insert(0, i);
            
            if (dishEndPos.y >= 0.0001f)
            {
                int idx = Mathf.RoundToInt((dishEndPos.x - DiningManager.Instance.DiningMinPosX) /
                                           MainSceneManager.Instance.PosXFactor);
                if (idx >= 0 && idx < DiningManager.Instance.CatCnt)
                {
                    InteractionManager.Instance.CatDishRelative[idx] = dish;
                }
            }
            dish.Rotate(dishEndPos, moveXFirst);
        }
    }

    public void CheckDishCondition()
    {
        if (currCompletedRotCnt < DishCnt - 1) { currCompletedRotCnt++; return; }
        
        currCompletedRotCnt = 0;

        if (_checkDishIdx.Count == 0)
        {
            bool isAllDishEmpty = true;
            foreach (DishBehaviour dish in DishBehaviourDict.Values)
            {
                if (dish.DishData.Sushi != SushiTypes.SushiEmpty)
                {
                    isAllDishEmpty = false;
                    break;
                }
            }

            Vector3 firstDishPos = DishBehaviourDict[0].DishData.CurrPos;

            bool passed = false;
            foreach (var pair in InteractionManager.Instance.passedDish)
                if (pair.Value != null) 
                    passed = true;

            if (isAllDishEmpty && firstDishPos.y < 0f && Mathf.Abs(firstDishPos.x - ServingMinPosX) <= 0.0001f)
            {
                MainSceneManager.Instance.isRotating = false;
                MainSceneManager.Instance.CheckClear();
                
                if (MainSceneManager.Instance._currRotateCnt == 0 || MainSceneManager.Instance.isClear)
                    MainSceneManager.Instance.ShowClearPanel();
                else
                {
                    foreach (string sushiStr in MainSceneManager.Instance._nextSushi)
                    {
                        if (Enum.TryParse(sushiStr, ignoreCase: true, out SushiTypes sushi))
                            CardManager.Instance.AddCard(sushi);
                    }
                }
            }
            else if (passed) InteractionManager.Instance.CheckCatDishRelative();
            else RotateDishOnce();
            return;
        }
        
        InteractionManager.Instance.CheckCatDishRelative();
    }

    public void GenerateSushi(SushiTypes sushiType)
    {
        bool isGenerated = false;
        
        foreach (DishBehaviour dish in DishBehaviourDict.Values)
        {
            if (dish.DishData.Sushi != SushiTypes.SushiEmpty) continue;
            isGenerated = true;
            dish.GenerateSushi(sushiType);
            break;
        }
        
        if (!isGenerated) InteractionManager.Instance.ActivateResult();
        else
        {
            all2 = 1;
            curr2 = 0;
        }
    }

    private int all2, curr2;
    
    // public void GenerateSushi(SushiTypes sushiType, ColorTypes dishColor)
    // {
    //     all2 = curr2 = 0;
    //     foreach (DishBehaviour dish in DishBehaviourDict.Values)
    //     {
    //         if (dish.DishData.Sushi != SushiTypes.Empty || dish.DishData.Color != dishColor) continue;
    //         all2++;
    //         dish.GenerateSushi(sushiType);
    //         break;
    //     }
    //     
    //     Debug.Log($"all: {all2}, curr: {curr2}");
    //
    //     if (all2 == 0) InteractionManager.Instance.ActivateResult();
    // }

    public void GenerateSushiCompleted(SushiTypes sushi)
    {
        if (curr2 < all2 - 1) { curr2++; return; }
        InteractionManager.Instance.EnQueueCondition(ConditionTypes.SushiGenerated, sushi.ToString());
        InteractionManager.Instance.ActivateResult();
    }

    public void ChangeType(SushiTypes sushi1, SushiTypes sushi2)
    {
        bool isGenerated = false;
        foreach (DishBehaviour dish in DishBehaviourDict.Values)
        {
            if (dish.DishData.Sushi != sushi1) continue;
            isGenerated = true;
            dish.PutSushiOnDish(sushi2);
            break;
        }
        if (isGenerated && sushi2 != SushiTypes.SushiEmpty)
        {
            InteractionManager.Instance.EnQueueCondition(ConditionTypes.SushiGenerated, sushi2.ToString());
            InteractionManager.Instance.TriggerProcess();
        }
    }

    public void EmptyNextDish(int catId)
    {
        DishBehaviour dish = InteractionManager.Instance.CatDishRelative[catId];
        if (dish == null)
        {
            InteractionManager.Instance.ActivateResult();
            return;
        }
        int emptyDishId = DishBehaviourDict.FirstOrDefault(pair => pair.Value == dish).Key;
        if (emptyDishId == DishCnt - 1)
        {
            InteractionManager.Instance.ActivateResult();
            return;
        }
        DishBehaviourDict[emptyDishId + 1].EmptyMotion();
    }

    public void EmptyColorDish(ColorTypes dishColor)
    {
        foreach (DishBehaviour dish in DishBehaviourDict.Values)
        {
            if (dish.DishData.Color == dishColor) dish.EmptyMotion();
        }
    }

    private int curr, all;
    public void ChangeSushiType(SushiTypes sushi1, SushiTypes sushi2)
    {
        curr = all = 0;
        foreach (DishBehaviour dish in DishBehaviourDict.Values)
        {
            if (dish.DishData.Sushi != sushi1) continue;
            all++;
            dish.ChangeMotion(sushi2);
        }
        if (all == 0) InteractionManager.Instance.ActivateResult();
    }

    public void EndChangeType()
    {
        if (curr < all - 1) { curr++; return; }
        InteractionManager.Instance.ActivateResult();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour, IInit
{
    public static TableManager Instance { get; private set; }
    
    [Header("Rail")]
    [SerializeField] private Sprite[] railSprites;
    [SerializeField] private GameObject railPref, railParent;
    
    [Header("Serving")]
    [SerializeField] private List<DishBehaviour> dishes;
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
        
        dishes = new List<DishBehaviour>();
        
        float currPosX = -1f * (DishCnt / 2) * MainSceneManager.Instance.PosXFactor;
        currPosX += RailCnt % 2 == 1 ? 0f : 0.5f * MainSceneManager.Instance.PosXFactor;
        ServingMinPosX = currPosX;
        
        for (int i = 0; i < DishCnt; i++)
        {
            GameObject tmpObj = Instantiate(dishPref, dishParent.transform);
            dishes.Add(tmpObj.GetComponent<DishBehaviour>());
            if (Enum.TryParse(dishColor[i], ignoreCase: true, out DishTypes color))
            {
                dishes[i].InitDish(SushiTypes.Empty, color, new Vector3(currPosX, _railMaxPosY * -1f, 0));
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
    
    public void RotateDishOnce()
    {
        Debug.LogWarning("RotateDishOnce");
        currCompletedRotCnt = 0;
        _checkDishIdx = new List<int>();
        
        for (int i = 0; i < dishes.Count; i++)
        {
            DishBehaviour dish = dishes[i];
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
                                             MainSceneManager.Instance.PosXFactor * DiningManager.Instance.CatCnt) <= 0.0001f)
                _checkDishIdx.Insert(0, i);
            
            dish.Rotate(dishEndPos, moveXFirst);
        }
    }

    public void CheckDishCondition()
    {
        if (currCompletedRotCnt < DishCnt - 1) { currCompletedRotCnt++; return; }

        string list = "";
        foreach(int idx in _checkDishIdx) list += idx + ", ";
        Debug.Log($"completed: {currCompletedRotCnt}, dishCnt: {DishCnt}, list: [{list}]");
        
        currCompletedRotCnt = 0;

        if (_checkDishIdx.Count == 0)
        {
            RotateDishOnce();
            return;
        }

        foreach (int idx in _checkDishIdx)
        {
            bool eatSushi = DiningManager.Instance.ActivateDishEffect(dishes[idx].DishData);
            if (eatSushi)
            {
                dishes[idx].Eat();
            }
        }
    }
}

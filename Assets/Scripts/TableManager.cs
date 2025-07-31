using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [Header("Rail")]
    [SerializeField] private int railCnt;
    [SerializeField] private GameObject railPref, railParent;
    [SerializeField] private Sprite[] railSprites;
    
    [Header("Serving")]
    [SerializeField] private int servingCnt;
    [SerializeField] private GameObject dishPref, dishParent;
    [SerializeField] private List<DishBehaviour> dishes;
    
    [Header("Dining")]
    [SerializeField] private int diningCnt;
    [SerializeField] private GameObject catPref, catParent;

    private readonly float _railMaxPosY = 1.35f, _catPosY = 3f, _posXFactor = 1.8f;
    private float _rotationMinPosX, _railMinPosX, _diningMinPosX, _servingMinPosX;

    private void Awake()
    {
        // _railSprites = Resources.LoadAll<Sprite>("Sprites/rails");
    }

    private void Start()
    {
        // railCnt = GameManager.Instance.railCnt;
        InitTable();
    }

    #region Initialization
    public void InitTable()
    {
        if (railCnt < 1 || servingCnt < 1 || diningCnt < 1) { Debug.LogError($"something's wrong [railCnt == {railCnt}, servingCnt == {servingCnt}, diningCnt == {diningCnt}]"); return; }
        if (servingCnt > railCnt) { Debug.LogError($"servingCnt is bigger than railCnt [railCnt == {railCnt}, servingCnt == {servingCnt}]"); return; }
        if (diningCnt > railCnt) { Debug.LogError($"diningCnt is bigger than railCnt [railCnt == {railCnt}, diningCnt == {diningCnt}]"); return; }
        
        GenerateRail();
        GenerateDish();
        GenerateCat();
    }

    private void GenerateRail()
    {
        float currPosX = -1f * (float)(railCnt / 2) * _posXFactor;
        currPosX -= railCnt % 2 == 1 ? _posXFactor : 0.5f * _posXFactor;
        
        _rotationMinPosX = currPosX;
        _railMinPosX = currPosX + _posXFactor;

        GameObject tmpObj;
        
        for (int i = -1; i < railCnt+1; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (j == 0)
                {
                    if (i == -1 || i == railCnt)
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
                    if (i == -1 || i == railCnt)
                    {
                        SpriteRenderer spr = tmpObj.GetComponent<SpriteRenderer>();
                        spr.sprite = railSprites[1];
                        spr.flipX = i == railCnt;
                        spr.flipY = j == -1;
                    }
                    else
                    {
                        tmpObj.GetComponent<SpriteRenderer>().sprite = railSprites[0];
                    }
                }
            }
            currPosX += _posXFactor;
        }
    }

    private void GenerateDish()
    {
        float currPosX = -1f * (float)(servingCnt / 2) * _posXFactor;
        currPosX -= railCnt % 2 == 1 ? 0f : 0.5f * _posXFactor;
        _servingMinPosX = currPosX;
        
        GameObject tmpObj;

        for (int i = 0; i < servingCnt; i++)
        {
            tmpObj = Instantiate(dishPref, dishParent.transform);
            dishes.Add(tmpObj.GetComponent<DishBehaviour>());
            dishes[i].InitDish(SushiTypes.Egg, new Vector3(currPosX, _railMaxPosY * -1f, 0));
            // tmpObj.GetComponent<SpriteRenderer>().sprite = ;
            currPosX += _posXFactor;
        }
    }

    private void GenerateCat()
    {
        float currPosX = -1f * (float)(diningCnt / 2) * _posXFactor;
        currPosX -= railCnt % 2 == 1 ? 0f : 0.5f * _posXFactor;
        _diningMinPosX = currPosX;
        
        GameObject tmpObj;

        for (int i = 0; i < diningCnt; i++)
        {
            tmpObj = Instantiate(catPref, catParent.transform);
            tmpObj.transform.localPosition = new Vector3(currPosX, _catPosY, 0);
            // tmpObj.GetComponent<SpriteRenderer>().sprite = ;
            currPosX += _posXFactor;
        }
    }
    #endregion

    public void RotateDish()
    {
        foreach (DishBehaviour dish in dishes)
        {
            Vector3 dishEndPos = Vector3.zero, dishStartPos = dish.CurrPos;
            
            if (Mathf.Abs(dishStartPos.x - _railMinPosX) <= 0.0001f && dishStartPos.y < 0f)
            {
                // 좌하단 -> 좌
                dishEndPos.x = _rotationMinPosX;
                dishEndPos.y = 0;
                dish.Rotate(dishEndPos, true);
            }
            else if (Mathf.Abs(dishStartPos.x - _rotationMinPosX) <= 0.0001f)
            {
                // 좌 -> 좌상단
                dishEndPos.x = _railMinPosX;
                dishEndPos.y = _railMaxPosY;
                dish.Rotate(dishEndPos, false);
            }
            else if (Mathf.Abs(dishStartPos.x + _railMinPosX) <= 0.0001f && dishStartPos.y > 0f)
            {
                // 우상단 -> 우
                dishEndPos.x = -1f * _rotationMinPosX;
                dishEndPos.y = 0;
                dish.Rotate(dishEndPos, true);
            }
            else if (Mathf.Abs(dishStartPos.x + _rotationMinPosX) <= 0.0001f)
            {
                // 우 -> 우하단
                dishEndPos.x = -1f * _railMinPosX;
                dishEndPos.y = -1f * _railMaxPosY;
                dish.Rotate(dishEndPos, false);
            }
            else if (dishStartPos.y < 0f)
            {
                // 밑줄
                dishEndPos.x = dishStartPos.x - _posXFactor;
                dishEndPos.y = dishStartPos.y;
                dish.Rotate(dishEndPos);
            }
            else
            {
                // 윗줄
                dishEndPos.x = dishStartPos.x + _posXFactor;
                dishEndPos.y = dishStartPos.y;
                dish.Rotate(dishEndPos);
            }
        }
    }
}

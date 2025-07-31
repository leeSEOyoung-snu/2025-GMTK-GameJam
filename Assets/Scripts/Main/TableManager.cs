using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour, IInit
{
    [Header("Rail")]
    [SerializeField] private Sprite[] railSprites;
    [SerializeField] private GameObject railPref, railParent;
    private int railCnt;
    
    [Header("Serving")]
    [SerializeField] private List<DishBehaviour> dishes;
    [SerializeField] private GameObject dishPref, dishParent;
    private int servingCnt;

    private readonly float _railMaxPosY = 1.35f;
    private float _rotationMinPosX, _railMinPosX;
    

    #region Initialization
    public void Init()
    {
        railCnt = (int)MainSceneManager.Instance.CurrStageData["RailCnt"];
        servingCnt = (int)MainSceneManager.Instance.CurrStageData["ServingCnt"];
        
        if (railCnt < 1 || servingCnt < 1) { Debug.LogError($"something's wrong [railCnt == {railCnt}, servingCnt == {servingCnt}]"); return; }
        if (servingCnt > railCnt) { Debug.LogError($"servingCnt is bigger than railCnt [railCnt == {railCnt}, servingCnt == {servingCnt}]"); return; }
        
        GenerateRail();
        GenerateDish();
    }

    private void GenerateRail()
    {
        foreach (Transform rail in railParent.transform)
            Destroy(rail.gameObject);
        
        float currPosX = -1f * (railCnt / 2) * MainSceneManager.Instance.PosXFactor;
        currPosX -= railCnt % 2 == 1 ? MainSceneManager.Instance.PosXFactor : 0.5f * MainSceneManager.Instance.PosXFactor;
        
        _rotationMinPosX = currPosX;
        _railMinPosX = currPosX + MainSceneManager.Instance.PosXFactor;

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
            currPosX += MainSceneManager.Instance.PosXFactor;
        }
    }

    private void GenerateDish()
    {
        foreach (Transform dish in dishParent.transform)
            Destroy(dish.gameObject);
        
        dishes = new List<DishBehaviour>();
        
        float currPosX = -1f * (servingCnt / 2) * MainSceneManager.Instance.PosXFactor;
        currPosX += railCnt % 2 == 1 ? 0f : 0.5f * MainSceneManager.Instance.PosXFactor;
        MainSceneManager.Instance.servingMinPosX = currPosX;
        
        GameObject tmpObj;

        for (int i = 0; i < servingCnt; i++)
        {
            tmpObj = Instantiate(dishPref, dishParent.transform);
            dishes.Add(tmpObj.GetComponent<DishBehaviour>());
            dishes[i].InitDish(SushiTypes.Egg, new Vector3(currPosX, _railMaxPosY * -1f, 0));
            // tmpObj.GetComponent<SpriteRenderer>().sprite = ;
            currPosX += MainSceneManager.Instance.PosXFactor;
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
                dishEndPos.x = dishStartPos.x - MainSceneManager.Instance.PosXFactor;
                dishEndPos.y = dishStartPos.y;
                dish.Rotate(dishEndPos);
            }
            else
            {
                // 윗줄
                dishEndPos.x = dishStartPos.x + MainSceneManager.Instance.PosXFactor;
                dishEndPos.y = dishStartPos.y;
                dish.Rotate(dishEndPos);
            }
        }
    }
}

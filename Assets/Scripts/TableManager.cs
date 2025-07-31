using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [Header("Table")]
    [SerializeField] private GameObject dishPref;
    [SerializeField] private GameObject tablePref, tableParent;
    [SerializeField] private int tableCnt, servingCnt, diningCnt;
    [SerializeField] private Sprite[] tableSprites;

    private readonly float posY = 1.35f, posXFactor = 1.8f;
    //private float 

    private void Awake()
    {
        // _tableSprites = Resources.LoadAll<Sprite>("Sprites/Tables");
    }

    private void Start()
    {
        // tableCnt = GameManager.Instance.tableCnt;
        InitTable();
    }

    public void InitTable()
    {
        if (tableCnt < 1 || servingCnt < 1 || diningCnt < 1) { Debug.LogError($"something's wrong [tableCnt == {tableCnt}, servingCnt == {servingCnt}, diningCnt == {diningCnt}]"); return; }
        if (servingCnt > tableCnt) { Debug.LogError($"servingCnt is bigger than tableCnt [tableCnt == {tableCnt}, servingCnt == {servingCnt}]"); return; }
        if (diningCnt > tableCnt) { Debug.LogError($"diningCnt is bigger than tableCnt [tableCnt == {tableCnt}, diningCnt == {diningCnt}]"); return; }
        
        GenerateTable();
        GenerateDish();
    }

    private void GenerateTable()
    {
        float currPosX = -1f * (float)(tableCnt / 2) * posXFactor;
        currPosX += tableCnt % 2 == 1 ? -1f * posXFactor : -0.5f * posXFactor;

        GameObject tmpObj;
        
        for (int i = -1; i < tableCnt+1; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Debug.Log($"i == {i}, j == {j}");
                if (j == 0)
                {
                    if (i == -1 || i == tableCnt)
                    {
                        tmpObj = Instantiate(tablePref, tableParent.transform);
                        tmpObj.transform.localPosition = new Vector3(currPosX, 0, 0);
                        tmpObj.GetComponent<SpriteRenderer>().sprite = tableSprites[2];
                    }
                }
                else
                {
                    tmpObj = Instantiate(tablePref, tableParent.transform);
                    tmpObj.transform.localPosition = new Vector3(currPosX, posY * j, 0);
                    if (i == -1 || i == tableCnt)
                    {
                        SpriteRenderer spr = tmpObj.GetComponent<SpriteRenderer>();
                        spr.sprite = tableSprites[1];
                        spr.flipX = i == tableCnt;
                        spr.flipY = j == -1;
                    }
                    else
                    {
                        tmpObj.GetComponent<SpriteRenderer>().sprite = tableSprites[0];
                    }
                }
            }
            currPosX += posXFactor;
        }
    }

    private void GenerateDish()
    {
        
    }
}

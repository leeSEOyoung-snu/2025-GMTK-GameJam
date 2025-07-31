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

    private readonly float posY = 1.6f, posXFactor = 2f;

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
        
        float currPosX = -1f * (float)(tableCnt / 2) * posXFactor;
        currPosX += tableCnt % 2 == 1 ? -1f * posXFactor : -0.5f * posXFactor;

        GameObject tmpObj; 
        tmpObj = Instantiate(tablePref, new Vector3(currPosX, posY, 0), Quaternion.identity, tableParent.transform);
        tmpObj.GetComponent<SpriteRenderer>().sprite = tableSprites[1];
        tmpObj = Instantiate(tablePref, new Vector3(currPosX, 0, 0), Quaternion.identity, tableParent.transform);
        tmpObj.GetComponent<SpriteRenderer>().sprite = tableSprites[2];
        tmpObj = Instantiate(tablePref, new Vector3(currPosX, -1f * posY, 0), Quaternion.identity, tableParent.transform);
        tmpObj.GetComponent<SpriteRenderer>().sprite = tableSprites[1];
        tmpObj.GetComponent<SpriteRenderer>().flipY = true;
        
        for (int i = 0; i < tableCnt; i++)
        {
            currPosX += 1f * posXFactor;
            tmpObj = Instantiate(tablePref, new Vector3(currPosX, posY, 0), Quaternion.identity, tableParent.transform);
            tmpObj.GetComponent<SpriteRenderer>().sprite = tableSprites[0];
            tmpObj = Instantiate(tablePref, new Vector3(currPosX, -1f * posY, 0), Quaternion.identity, tableParent.transform);
            tmpObj.GetComponent<SpriteRenderer>().sprite = tableSprites[0];
        }
        
        currPosX += 1f * posXFactor;
        tmpObj = Instantiate(tablePref, new Vector3(currPosX, posY, 0), Quaternion.identity, tableParent.transform);
        tmpObj.GetComponent<SpriteRenderer>().sprite = tableSprites[1];
        tmpObj.GetComponent<SpriteRenderer>().flipX = true;
        tmpObj = Instantiate(tablePref, new Vector3(currPosX, 0, 0), Quaternion.identity, tableParent.transform);
        tmpObj.GetComponent<SpriteRenderer>().sprite = tableSprites[2];
        tmpObj = Instantiate(tablePref, new Vector3(currPosX, -1f * posY, 0), Quaternion.identity, tableParent.transform);
        tmpObj.GetComponent<SpriteRenderer>().sprite = tableSprites[1];
        tmpObj.GetComponent<SpriteRenderer>().flipX = true;
        tmpObj.GetComponent<SpriteRenderer>().flipY = true;
    }
}

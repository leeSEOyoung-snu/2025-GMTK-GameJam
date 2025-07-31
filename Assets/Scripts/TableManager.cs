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
    
    [Header("Dining")]
    [SerializeField] private int diningCnt;
    [SerializeField] private GameObject catPref, catParent;

    private readonly float _railPosY = 1.35f, _catPosY = 3f, _posXFactor = 1.8f;
    private float _rotationMinPosX, _diningMinPosX, _servingMinPosX;

    private void Awake()
    {
        // _railSprites = Resources.LoadAll<Sprite>("Sprites/rails");
    }

    private void Start()
    {
        // railCnt = GameManager.Instance.railCnt;
        InitTable();
    }

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
                    tmpObj.transform.localPosition = new Vector3(currPosX, _railPosY * j, 0);
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
        _servingMinPosX = currPosX;
        
        GameObject tmpObj;

        for (int i = 0; i < servingCnt; i++)
        {
            tmpObj = Instantiate(dishPref, dishParent.transform);
            tmpObj.transform.localPosition = new Vector3(currPosX, _railPosY * -1f, 0);
            // tmpObj.GetComponent<SpriteRenderer>().sprite = ;
            currPosX += _posXFactor;
        }
    }

    private void GenerateCat()
    {
        float currPosX = -1f * (float)(diningCnt / 2) * _posXFactor;
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

    public void RotateDish()
    {
        
    }
}

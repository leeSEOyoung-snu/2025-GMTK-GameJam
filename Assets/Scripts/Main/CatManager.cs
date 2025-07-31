using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour, IInit
{
    [SerializeField] private GameObject catPref, catParent;
    
    private int railCnt, diningCnt;
    private readonly float _catPosY = 3f;
    private List<Dictionary<string, object>> currCatData;
    
    public void Init()
    {
        currCatData = GameManager.Instance.GetCatData();
        
        railCnt = (int)MainSceneManager.Instance.CurrStageData["RailCnt"];
        diningCnt = currCatData.Count;
        
        if (railCnt < 1 || diningCnt < 1) { Debug.LogError($"something's wrong [railCnt == {railCnt}, diningCnt == {diningCnt}]"); return; }
        if (diningCnt > railCnt) { Debug.LogError($"diningCnt is bigger than railCnt [railCnt == {railCnt}, diningCnt == {diningCnt}]"); return; }
        
        GenerateCat();
    }
    
    private void GenerateCat()
    {
        foreach (Transform cat in catParent.transform)
            Destroy(cat.gameObject);
        
        float currPosX = -1f * (diningCnt / 2) * MainSceneManager.Instance.PosXFactor;
        currPosX += railCnt % 2 == 1 ? 0f : 0.5f * MainSceneManager.Instance.PosXFactor;
        MainSceneManager.Instance.diningMinPosX = currPosX;
        
        GameObject tmpObj;

        for (int i = 0; i < diningCnt; i++)
        {
            tmpObj = Instantiate(catPref, catParent.transform);
            tmpObj.transform.localPosition = new Vector3(currPosX, _catPosY, 0);
            // tmpObj.GetComponent<SpriteRenderer>().sprite = ;
            currPosX += MainSceneManager.Instance.PosXFactor;
        }
    }
}

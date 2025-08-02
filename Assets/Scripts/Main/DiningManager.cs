using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiningManager : MonoBehaviour, IInit
{
    public static DiningManager Instance { get; private set; }
    
    [SerializeField] private GameObject catPref, catParent;
    public List<Sprite> catSprites;
    
    public int CatCnt { get; private set; }
    public float DiningMinPosX { get; private set; }
    private readonly float _catPosY = 3f;
    private List<Dictionary<string, object>> currCatData;
    
    public Dictionary<int, CatBehaviour> CatBehaviourDict { get; private set; }
    
    public readonly float BubbleTypePosX = 0.5f;
    public readonly float BubbleSushiScale = 0.6f, BubbleDishScale = 0.4f, BubbleHighlightFactor = 1.2f;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    
    public void Init()
    {
        currCatData = GameManager.Instance.GetCatData();
        
        CatCnt = currCatData.Count;
        
        if (TableManager.Instance.RailCnt < 1 || CatCnt < 1) { Debug.LogError($"something's wrong [TableManager.Instance.RailCnt == {TableManager.Instance.RailCnt}, CatCnt == {CatCnt}]"); return; }
        if (CatCnt > TableManager.Instance.RailCnt) { Debug.LogError($"CatCnt is bigger than TableManager.Instance.RailCnt [TableManager.Instance.RailCnt == {TableManager.Instance.RailCnt}, CatCnt == {CatCnt}]"); return; }
        
        GenerateCat();
    }
    
    private void GenerateCat()
    {
        foreach (Transform cat in catParent.transform)
            Destroy(cat.gameObject);
        
        float currPosX = -1f * (CatCnt / 2) * MainSceneManager.Instance.PosXFactor;
        currPosX += TableManager.Instance.RailCnt % 2 == 1 ? 0f : 0.5f * MainSceneManager.Instance.PosXFactor;
        DiningMinPosX = currPosX;
        
        CatBehaviourDict = new Dictionary<int, CatBehaviour>();
        
        for (int i = 0; i < CatCnt; i++)
        {
            CatBehaviourDict.Add(i, Instantiate(catPref, catParent.transform).GetComponent<CatBehaviour>());
            CatBehaviourDict[i].InitCat(new Vector3(currPosX, _catPosY, 0), currCatData[i], i);
            currPosX += MainSceneManager.Instance.PosXFactor;
        }
    }

    public bool ActivateDishEffect(Dish dishData)
    {
        return true;
    }
}

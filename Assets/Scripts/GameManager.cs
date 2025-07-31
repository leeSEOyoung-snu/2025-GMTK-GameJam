using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singleton instance
    public static GameManager Instance { get; private set; }
    
    private List<Dictionary<string, object>> _stageData;
    private List<Dictionary<string, object>> _catData;
    private string StageFileName;
    
    public int CurrStageIdx { get; private set; }
    public readonly float RotateDuration = 1f;
    
    private void Awake()
    {
        // Ensure that there is only one instance of GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance across scenes
            Init();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void Init()
    {
        _stageData = CSVReader.Read("Data/Test");
        _catData = CSVReader.Read("Data/TestCat");
        CurrStageIdx = 0;
    }

    public Dictionary<string, object> GetStageData()
    {
        return _stageData[CurrStageIdx];
    }

    public List<Dictionary<string, object>> GetCatData()
    {
        bool goodToEnd = false;
        List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
        foreach (var catData in _catData)
        {
            if ((int)catData["Stage"] != (int)_stageData[CurrStageIdx]["Stage"])
            {
                if (goodToEnd) break;
            }
            else
            {
                goodToEnd = true;
                result.Add(catData);
            }
        }
        return result;
    }

    public void EndStage()
    {
        CurrStageIdx++;
        Debug.Log("Curr Stage Idx: " + CurrStageIdx);
    }
    
    #region filname
    
    public void SetStageFileName(string fileName)
    {
        StageFileName = fileName;
        Debug.Log("GM : Stage File Name Set: " + StageFileName);
    }
    #endregion
}

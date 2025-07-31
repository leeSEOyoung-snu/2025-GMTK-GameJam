using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singleton instance
    public static GameManager Instance { get; private set; }

    private List<Dictionary<string, object>> _stageData;
    
    public int CurrStageIdx { get; private set; }
    
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
        CurrStageIdx = 0;
    }

    public Dictionary<string, object> GetStageData()
    {
        return _stageData[CurrStageIdx];
    }

    public void EndStage()
    {
        CurrStageIdx++;
        Debug.Log("Curr Stage Idx: " + CurrStageIdx);
    }
}

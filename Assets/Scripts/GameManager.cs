using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //singleton instance
    public static GameManager Instance { get; private set; }
    
    private List<Dictionary<string, object>> _stageData;
    private List<Dictionary<string, object>> _catData;
    private string StageFileName;
    private bool isClear;
    public int CurrStageIdx { get; private set; }
    public readonly float RotateDuration = 1f;
    

    [SerializeField] public GameObject StageSummaryPanel;
    //Save Part
    [SerializeField] public List<int> StageCount;
    public SaveData _saveData;
    private string StageData;
    private string path;
    
    private void Awake()
    {
        // Ensure that there is only one instance of GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance across scenes
            path = Path.Combine(Application.persistentDataPath, "saveData.json");
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
        LoadSaveData();
        CurrStageIdx = 0;
    }

    public void SetStageData(int BigStage, int SmallStage)
    {
        StageData = BigStage.ToString() +"0"+ SmallStage.ToString();
        //then load Scene
        SceneManager.LoadScene("Scenes/Test SEO");
    }

    public Dictionary<string, object> GetStageData(){
        foreach(Dictionary<string,object> d in _stageData) {
            if (int.Parse(StageData) == (int)d["Stage"]) {
                return d;
            }
        } 
        Debug.LogError("Stage data not found for Stage: " + StageData);
        return new Dictionary<string, object>();
    }
    
    public List<Dictionary<string, object>> GetCatData()
    {
        bool goodToEnd = false;
        List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
        foreach (var catData in _catData)
        {
            if (  int.Parse(StageData) != (int)catData["Stage"])
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

    public void StageSummaryPanelOn()
    {
        StageSummaryPanel.SetActive(true);
        StageSummaryPanel.GetComponent<SummaryBehaviour>().SetSummary(
            isClear, MainSceneManager.Instance._currScore, MainSceneManager.Instance._targetScore);
    }
    
    public void EndStage()  //gotoNextStage
    {
        CurrStageIdx++;
        Debug.Log("Curr Stage Idx: " + CurrStageIdx);
    }
    
    #region Save

    private void LoadSaveData()
    {
        if (File.Exists(path)) 
        {
            string json = File.ReadAllText(path);
            _saveData = JsonConvert.DeserializeObject<SaveData>(json);
            Debug.Log("Save data loaded from " + path);
        }
        else
        {
            initSaveData();
            SaveSaveData();
            Debug.Log("Save data initialized and saved to " + path);
        }
    }

    private void SaveSaveData()
    {
        string json = JsonConvert.SerializeObject(_saveData, Formatting.Indented);
        if (json == null)
        {
            Debug.Log("FUUUUUCKKK!!!!");
        }
        File.WriteAllText(path, json);
        Debug.Log("Saved : "+ path);
    }

    private void initSaveData()
    {
        _saveData = new SaveData();
        _saveData.saveData = new List<List<int>>();
        _saveData.BigStageCount = StageCount.Count;
        _saveData.SmallStageCount = new List<int>(StageCount);  //Shallow copy of StageCount
        for (int i = 0; i < _saveData.BigStageCount; i++)
        {
            _saveData.saveData.Add(new List<int>());
            if (_saveData.saveData[i] == null) Debug.LogError("FUck!!!");
            
            for (int j = 0; j < _saveData.SmallStageCount[i]; j++)
            {
                _saveData.saveData[i].Add(0);
            }
        }
        
        //UnLock Hard Coding fuck
        for (int i = 0; i < _saveData.BigStageCount; i++)
        {
            _saveData.saveData[i][0] = 1;    //1st stage is always unlocked
        }
    }
    
    #endregion
}

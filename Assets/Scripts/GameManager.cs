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
    
    private List<Dictionary<string, object>> _currStageData;
    private List<Dictionary<string, object>> _catData;
    private string StageFileName;
    private bool isClear;
    public int CurrStageIdx { get; private set; }
    public readonly float RotateDuration = 1f;
    

    //Save Part
    [SerializeField] public List<int> StageCount;
    public SaveData _saveData;
    private string currStageData;
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
        _currStageData = CSVReader.Read("Data/Test");
        _catData = CSVReader.Read("Data/TestCat");

        LoadSaveData();
#if UNITY_EDITOR
        SetcurrStageData(1,1, true);
#endif
        CurrStageIdx = 0;
    }

    public void SetcurrStageData(int BigStage, int SmallStage, bool justSet)
    {
        CurrStageIdx = (BigStage-1)*10 + SmallStage - 1; // 0-indexed
        currStageData = BigStage.ToString() +"0"+ SmallStage.ToString();
        //then load Scene
        SoundManager.Instance.PlayBGM(SoundManager.Instance.BGMs[1]);
        if (justSet == false)
        {
            SceneManager.LoadScene("Scenes/Test SEO 1");
        }
    }

    public void SetcurrStageData(int StageIdx, bool justSet)
    {
        CurrStageIdx = StageIdx;
        currStageData = _currStageData[CurrStageIdx]["Stage"].ToString();
        //then load Scene
        SoundManager.Instance.PlayBGM(SoundManager.Instance.BGMs[1]);
        if (justSet == false)
        {
            SceneManager.LoadScene("Scenes/Test SEO 1");
        } 
    }

    public Dictionary<string, object> GetcurrStageData()
    {
        return _currStageData[CurrStageIdx];
        //I don't know why this code works.... but it's still working We dont't need to modify anymore.
        foreach(Dictionary<string,object> d in _currStageData) {
            if (int.Parse(currStageData) == (int)d["Stage"]) {
                return d;
            }
        } 
        Debug.LogError("Stage data not found for Stage: " + currStageData);
        return new Dictionary<string, object>();
    }
    
    public List<Dictionary<string, object>> GetCatData()
    {
        bool goodToEnd = false;
        List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
        foreach (var catData in _catData)
        {
            if (  int.Parse(currStageData) != (int)catData["Stage"])
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
        Debug.Log("EndStage button pressed");  // 버튼 눌렸는지 확인
        UpdateClearData();
        unlockStage();
        SaveSaveData();
        LoadSaveData();

        CurrStageIdx++;
        Debug.Log("Curr Stage Idx: " + CurrStageIdx);
        SetcurrStageData(CurrStageIdx, false);
    
        if (MainSceneManager.Instance == null)
            Debug.LogError("MainSceneManager.Instance is null");
        else
            MainSceneManager.Instance.Init();
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

    private void UpdateClearData()
    {
        int BS = int.Parse(currStageData.Substring(0, 1));
        int SS = int.Parse(currStageData.Substring(1));

        _saveData.saveData[BS-1][SS-1] = 2;
        Debug.Log(BS+"-"+SS+"has cleard");
    }

    private void unlockStage()   //call when stage is cleared
    {
        foreach (var s in _currStageData)
        {
            if(int.Parse(s["Stage"].ToString()) == int.Parse(currStageData))
            {
                if (int.Parse(s["Unlock"].ToString()) == -1) return;
                
                string[] tmp = s["Unlock"].ToString().Split('$');
                foreach (var t in tmp)
                {
                    int BS = int.Parse(t.Substring(0, 1));  //Chapter Can be smaller thaan 10
                    int SS = int.Parse(t.Substring(1));
                    
                    _saveData.saveData[BS-1][SS-1] = 1;
                    Debug.Log($"Stage {BS}0{SS} unlocked.");
                }
                break;
            }
        }
    }
}

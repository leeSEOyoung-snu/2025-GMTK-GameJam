using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.SceneManagement;

public enum StageHeader
{
    Chapter,
    Stage,
    Rail,
    Rotate,
    Dish,
    StartCard,
    NextSushi,
    Menu,
    Score,
    NewIcon,
    Description,
    Unlock,
}

public enum CatHeader
{
    Chapter,
    Stage,
    Sprite,
    Color,
    Condition,
    ConVal1,
    ConVal2,
    Result,
    ResVal1,
    ResVal2,
}

public class GameManager : MonoBehaviour
{
    //singleton instance
    public static GameManager Instance { get; private set; }

    private Dictionary<int, Dictionary<int, Dictionary<StageHeader, object>>> _stageData;
    private Dictionary<int, Dictionary<int, List<Dictionary<CatHeader, object>>>> _catData;
    
    private bool isClear;

    private int currChapter, currStage;
    public readonly float RotateDuration = 0.6f;
    

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
            Init();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void Init()
    {
        _stageData = CSVReader.ReadStageData();
        _catData = CSVReader.ReadCatData();

        path = Path.Combine(Application.persistentDataPath, "saveData.json");
        LoadSaveData();
        
        currChapter = 1;
        currStage = 1;
#if UNITY_EDITOR
        SetcurrStageData(3,1, true);
#endif
    }

    public void SetcurrStageData(int chapter, int stage, bool justSet)
    {
        currChapter = chapter;
        currStage = stage;

        while (true)
        {
            if (!_stageData.Keys.Contains(currChapter))
            {
                Debug.LogError($"No Chapter Found: chapter{currChapter}");
                currChapter++;
                currStage = 1;
            }
            else if (currStage > _stageData[currChapter].Count)
            {
                currChapter++;
                currStage = 1;
            }
            else if (!_stageData[currChapter].Keys.Contains(currStage))
            {
                Debug.LogError($"No Stage Found: chapter{currChapter} - stage{currStage}");
                currChapter++;
                currStage = 1;
            }
            else break;
        }

        currStageData = $"{chapter}{stage:D4}";
        //then load Scene
        SoundManager.Instance.PlayBGM(SoundManager.Instance.BGMs[1]);
        if (justSet == false)
        {
            SceneManager.LoadScene("Scenes/Test SEO 1");
        }
    }

    public Dictionary<StageHeader, object> GetcurrStageData()
    {
        return _stageData[currChapter][currStage];
        //I don't know why this code works.... but it's still working We dont't need to modify anymore.
        // foreach(Dictionary<string,object> d in _currStageData) {
        //     if (int.Parse(currStageData) == (int)d["Stage"]) {
        //         return d;
        //     }
        // } 
        // Debug.LogError("Stage data not found for Stage: " + currStageData);
        // return new Dictionary<string, object>();
    }
    
    public List<Dictionary<CatHeader, object>> GetCatData()
    {
        return _catData[currChapter][currStage];
    }

    
    
    public void EndStage()
    {
        Debug.Log("EndStage button pressed");  // 버튼 눌렸는지 확인
        UpdateClearData();
        unlockStage();
        SaveSaveData();
        LoadSaveData();

        currChapter++;
        currStage++;
        Debug.Log($"Next Chapter: {currChapter}, Next Stage: {currStage}");
        SetcurrStageData(currChapter, currStage, false);
    
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
        if (_stageData.TryGetValue(currChapter, out var stages))
        {
            if (stages.TryGetValue(currStage, out var stage))
            {
                if (int.Parse(stage[StageHeader.Unlock].ToString()) == -1) return;
                
                string[] tmp = stage[StageHeader.Unlock].ToString().Split('$');
                foreach (var t in tmp)
                {
                    int BS = int.Parse(t.Substring(0, 1));  //Chapter Can be smaller thaan 10
                    int SS = int.Parse(t.Substring(1));
                    
                    _saveData.saveData[BS-1][SS-1] = 1;
                    Debug.Log($"Stage {BS}0{SS} unlocked.");
                }
            }
            else Debug.Log($"Stage Data에 currStage가 매칭되지 않음: currStage == {currStage}");
        }
        else Debug.Log($"Stage Data에 currChapter가 매칭되지 않음: currChapter == {currChapter}");
    }
}

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum SushiTypes { Egg = 0, Shrimp = 1, Unagi = 2, Tuna = 3, Maki = 4, Empty = 5 }
public enum DishTypes { W = 0, R = 1, Y = 2, B = 3 }

public class MainSceneManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI rotateCntText;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    public static MainSceneManager Instance { get; private set; }
    
    public Dictionary<string, object> CurrStageData { get; private set; }
    public readonly float PosXFactor = 1.8f;
    private List<IInit> _initScripts = new List<IInit>();
    
    public float RotateSpeedFactor { get; private set; }
    private int _maxRotateCnt, _currRotateCnt;
    private int _targetScore, _currScore;
    
    // TODO: 가격 수정
    public readonly Dictionary<SushiTypes, int> Price = new Dictionary<SushiTypes, int>()
    {
        { SushiTypes.Egg, 5 },
        { SushiTypes.Shrimp, 7 },
        { SushiTypes.Unagi, 10 },
        { SushiTypes.Tuna, 11 },
        { SushiTypes.Maki, 12 },
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _initScripts = new List<IInit>(transform.GetComponentsInChildren<IInit>());
        }
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        CurrStageData = GameManager.Instance.GetStageData();
        
        _maxRotateCnt = _currRotateCnt = (int)CurrStageData["RotateCnt"];
        UpdateRotateCnt();

        RotateSpeedFactor = 1f;
        
        _targetScore = (int)CurrStageData["TargetScore"];
        _currScore = 0;
        UpdateScore();
        
        foreach(IInit script in _initScripts) script.Init();
    }

    public void UpdateRotateCnt()
    {
        rotateCntText.text = $"Rotate Cnt [{_currRotateCnt} / {_maxRotateCnt}]";
    }

    public void UpdateScore()
    {
        scoreText.text = $"Score [{_currScore}/{_targetScore}]";
    }

    
}

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SushiTypes { Egg = 0, Shrimp = 1, Unagi = 2, Tuna = 3, Maki = 4, Empty = 5, Any = 6, SushiStandBy = 7 }
public enum ColorTypes { W = 0, R = 1, Y = 2, B = 3, DishStandBy = 4 }

public class MainSceneManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI rotateCntText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject startCookButton;
    
    public static MainSceneManager Instance { get; private set; }
    
    public Dictionary<string, object> CurrStageData { get; private set; }
    public readonly float PosXFactor = 1.75f;
    private List<IInit> _initScripts = new List<IInit>();
    
    public float RotateSpeedFactor { get; private set; }
    private int _maxRotateCnt, _currRotateCnt;
    private int _targetScore, _currScore;
    public bool isRotating;
    
    public bool CookStarted { get; private set; }
    
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
        
        isRotating = false;

        CookStarted = false;
        startCookButton.SetActive(true);
        
        foreach(IInit script in _initScripts) script.Init();
    }

    public void UpdateRotateCnt()
    {
        rotateCntText.text = _currRotateCnt.ToString();
    }

    public void UpdateScore()
    {
        scoreText.text = $"Score [{_currScore}/{_targetScore}]";
    }

    public void StartCook()
    {
        CookStarted = true;
        startCookButton.SetActive(false);
        TableManager.Instance.ReadyToCook();
    }

    public void Rotate()
    {
        if (!CookStarted || isRotating || _currRotateCnt == 0) return;
        isRotating = true;
        _currRotateCnt--;
        UpdateRotateCnt();
        TableManager.Instance.RotateDishOnce();
    }

    public void CheckConditionCompleted()
    {
        if (!isRotating) return;
        TableManager.Instance.RotateDishOnce();
    }
}

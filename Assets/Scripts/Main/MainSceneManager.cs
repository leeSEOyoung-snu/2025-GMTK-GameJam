using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum SushiTypes { Egg = 0, Shrimp = 1, Unagi = 2, Tuna = 3, Maki = 4, Empty = 5 }
public enum DishTypes { W = 0, R = 1, Y = 2, B = 3 }

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager Instance { get; private set; }
    
    public Dictionary<string, object> CurrStageData { get; private set; }
    public readonly float PosXFactor = 1.8f;
    private List<IInit> _initScripts = new List<IInit>();
    
    [Header("Rotate")]
    [SerializeField] private TextMeshProUGUI rotateCntText;
    public float RotateSpeedFactor { get; private set; }
    private int _maxRotateCnt, _currRotateCnt;

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
        rotateCntText.text = $"Rotate Cnt [{_currRotateCnt} / {_maxRotateCnt}]";

        RotateSpeedFactor = 1f;
        
        foreach(IInit script in _initScripts) script.Init();
    }
}

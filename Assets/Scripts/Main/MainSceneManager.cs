using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager Instance { get; private set; }
    
    public Dictionary<string, object> CurrStageData { get; private set; }
    private List<IInit> _initScripts = new List<IInit>();
    
    [Header("Rotate")]
    [SerializeField] private TextMeshProUGUI rotateCntText;
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
        
        foreach(IInit script in _initScripts) script.Init();
    }
}

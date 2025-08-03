using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SushiTypes { Egg = 0, Shrimp = 1, Unagi = 2, Tuna = 3, Maki = 4, Empty = 5, Any = 6, SushiStandBy = 7 }
public enum ColorTypes { W = 0, R = 1, Y = 2, B = 3, DishStandBy = 4 }

public class MainSceneManager : MonoBehaviour
{

    [Header("Prefabs")]
    [SerializeField] private GameObject popupPrefab;

    [SerializeField] private GameObject CardImagePrefab;
    [Header("References")]
    [SerializeField] private TextMeshProUGUI rotateCntText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject startCookButton;
    [SerializeField] private GameObject nextSushiPanel;
    [SerializeField] private Canvas mainCanvas;
    public static MainSceneManager Instance { get; private set; }
    
    public Dictionary<string, object> CurrStageData { get; private set; }
    public readonly float PosXFactor = 1.75f;
    private List<IInit> _initScripts = new List<IInit>();
    
    public float RotateSpeedFactor { get; private set; }
    private int _maxRotateCnt, _currRotateCnt;
    public int _targetScore, _currScore;
    private List<int> _newIcon;
    private List<string> _newIconDescription;
    private List<string> _nextSushi;
    public bool isRotating;
    
    public bool CookStarted { get; private set; }
    
    // TODO: 가격 수정
    public readonly Dictionary<SushiTypes, int> Price = new Dictionary<SushiTypes, int>()
    {
        { SushiTypes.Egg, 10 },
        { SushiTypes.Shrimp, 15 },
        { SushiTypes.Unagi, 20 },
        { SushiTypes.Tuna, 25 },
        { SushiTypes.Maki, -10 },
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
        CurrStageData = GameManager.Instance.GetcurrStageData();
        
        _maxRotateCnt = _currRotateCnt = (int)CurrStageData["RotateCnt"];
        UpdateRotateCnt();

        RotateSpeedFactor = 1f;
        
        _targetScore = (int)CurrStageData["TargetScore"];
        _currScore = 0;
        UpdateScore();
        
        //nextSushi
        _nextSushi = new List<string>();
        foreach (string i in CurrStageData["NextSushi"].ToString().Split('$'))
        {
            if (i == "X")  //there is no next Sushi
            {
                _nextSushi.Clear();
                break;
            }
            _nextSushi.Add(i);
        }
        initNextSushi();
        
        //newIcon
        _newIcon = new List<int>();
        foreach (string i in CurrStageData["NewIcon"].ToString().Split('_'))
        {
            if (i == "-1")  //there is no new Popup
            {
                _newIcon.Clear();
                break;
            }
            _newIcon.Add(int.Parse(i));
        }
        
        //newIconDescription
        _newIconDescription = new List<string>();
        foreach (string i in CurrStageData["Description"].ToString().Split('$'))
        {
            if (i == "NULL")  //there is no new Popup
            {
                _newIconDescription.Clear();
                break;
            }
            _newIconDescription.Add(i);
        }
        
        Madepopup();
        
        isRotating = false;

        CookStarted = false;
        startCookButton.SetActive(true);
        
        foreach(IInit script in _initScripts) script.Init();
    }

    public void UpdateRotateCnt()
    {
        rotateCntText.text = _currRotateCnt.ToString();
    }

    public void ChangeScore(int delta)
    {
        _currScore += delta;
        UpdateScore();
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
        if(_currRotateCnt == 1){ Destroy(nextSushiPanel);}
        UpdateRotateCnt();
        TableManager.Instance.RotateDishOnce();
    }

    public void CheckConditionCompleted()
    {
        Vector3 firstDishPos = TableManager.Instance.DishBehaviourDict[0].DishData.CurrPos;
        if (firstDishPos.y <= 0.0001f && Mathf.Abs(firstDishPos.x - TableManager.Instance.ServingMinPosX) <= 0.0001f)
        {
            foreach (CatBehaviour cat in DiningManager.Instance.CatBehaviourDict.Values)
                cat.isFull = false;
        }
        if (!isRotating) return;
        TableManager.Instance.RotateDishOnce();
    }

    private void Madepopup()
    {
        foreach(int i in _newIcon)
        {
            GameObject popup = Instantiate(popupPrefab, transform);
            popup.transform.SetParent(mainCanvas.transform, false);
            popup.GetComponent<PopupBehaviour>().InitPopup(i-1, _newIconDescription[i-1]);
        }
    }

    private void initNextSushi()
    {
        switch (_nextSushi.Count)
        {
            case 0:
                Destroy(nextSushiPanel);
                break;
            case 1:
                GameObject go = Instantiate(CardImagePrefab, nextSushiPanel.transform);
                go.transform.SetParent(nextSushiPanel.transform, false);
                go.transform.position += new Vector3(0, -30, 0);
                Enum.TryParse<SushiTypes>(_nextSushi[0], out SushiTypes sushiType);
                go.GetComponent<Image>().sprite = CardManager.Instance.cardSprites[(int)sushiType];
                break;
            case 2:
                GameObject go1 = Instantiate(CardImagePrefab, nextSushiPanel.transform);
                GameObject go2 = Instantiate(CardImagePrefab, nextSushiPanel.transform);
                go1.transform.SetParent(nextSushiPanel.transform, false);
                go2.transform.SetParent(nextSushiPanel.transform, false);
                go1.transform.position += new Vector3(-80, -30, 0);
                go2.transform.position += new Vector3(80, -30, 0);
                Enum.TryParse<SushiTypes>(_nextSushi[0], out SushiTypes sushiType1);
                Enum.TryParse<SushiTypes>(_nextSushi[1], out SushiTypes sushiType2);
                go1.GetComponent<Image>().sprite = CardManager.Instance.cardSprites[(int)sushiType1];
                go2.GetComponent<Image>().sprite = CardManager.Instance.cardSprites[(int)sushiType2];
                break;
            default:
                Debug.LogError("There is fucking somthing wrong with csv of nextSushi!!!");
                break;
        }
    }
}

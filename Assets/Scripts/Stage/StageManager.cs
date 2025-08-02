using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;

public class StageManager : MonoBehaviour
{
    #region Fields
    private int currentSelectedStage = 0;
    private string selectedStageFileName;
    private List<RectTransform> StagePanelsRectTransforms = new List<RectTransform>();
    private SaveData _saveData;
    
    Sequence stagePanelSequence;
    [SerializeField] private GameObject TitlePanel;
    [SerializeField] private GameObject OptionPanel;
    [Header("Stage Panels Size")]
    [SerializeField] private float BigStagePanelSizeX;
    [SerializeField] private float BigStagePanelSizeY;
    [SerializeField] private float SmallStagePanelSizeX;
    [SerializeField] private float SmallStagePanelSizeY;
    [SerializeField] private float StagePanelGap;
    [SerializeField] private float StagePanelMoveDuration;
    
    [Header("Stage Panels Objects")]
    [SerializeField] private GameObject StageSet;
    [SerializeField] private List<GameObject> StagePanels = new List<GameObject>();

    [Header("Buttons")] 
    [SerializeField] private GameObject TitleButton;
    [SerializeField] private GameObject OptionButton;
    [SerializeField] private GameObject LeftButton;
    [SerializeField] private GameObject RightButton;
    #endregion

    #region LifeCycle
    private void Awake()
    {
        stagePanelSequence = DOTween.Sequence();
        foreach (var st in StagePanels)
        {
            StagePanelsRectTransforms.Add(st.GetComponent<RectTransform>());
        }
    }
    
    private void Start()
    {
        // Initialize the stage panels
        ResizeStagePanels();
        _saveData = GameManager.Instance._saveData;
        for(int i = 0; i < StagePanels.Count; i++)
        {
            StagePanels[i].GetComponent<StagePanelBehaviour>().StageNodeData = _saveData.saveData[i];
        }
        LeftButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RightStageButtonClicekd();
                // Debug.Log("Current Selected Panel: " + currentSelectedStage);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                LeftStageButtonClicekd();
            //     Debug.Log("Current Selected Panel: " + currentSelectedStage);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TitleButtonClicked();
            }
    }

    #endregion

    #region DOTween Methods

    public void RightStageButtonClicekd()
    {
        if (stagePanelSequence == null || stagePanelSequence.IsPlaying()) return;
        
        stagePanelSequence = DOTween.Sequence();
        stagePanelSequence.Append(DOTween.To(() => 0f, x => { }, 0f, 0f)); //dummy tween to ensure the sequence is not empty
        RightStage();
        ResizeStagePanels();
        stagePanelSequence.Play();
        
    }
    
    public void LeftStageButtonClicekd()
    {
        if (stagePanelSequence == null || stagePanelSequence.IsPlaying()) return;
        stagePanelSequence = DOTween.Sequence();
        stagePanelSequence.Append(DOTween.To(() => 0f, x => { }, 0f, 0f));  //dummy tween to ensure the sequence is not empty
        LeftStage();
        ResizeStagePanels();
        stagePanelSequence.Play();
    }
    
    private void RightStage()
    {
        currentSelectedStage++;
        
        if (currentSelectedStage > StagePanels.Count - 1)
        {
            currentSelectedStage = StagePanels.Count - 1;
            return;
        }
        if(currentSelectedStage == StagePanels.Count - 1) RightButton.SetActive(false); 
        else RightButton.SetActive(true);
        if(currentSelectedStage > 0) LeftButton.SetActive(true);
        else LeftButton.SetActive(false);
        
        foreach (RectTransform rectPanels in StagePanelsRectTransforms)
        {
            stagePanelSequence.Join(rectPanels.DOMove(new Vector2(rectPanels.transform.position.x - StagePanelGap, rectPanels.transform.position.y), StagePanelMoveDuration));
        }
    }
    private void LeftStage()
    {
        currentSelectedStage--;
        if (currentSelectedStage < 0)
        {
            currentSelectedStage = 0;
            return;
        }

        if (currentSelectedStage == 0) LeftButton.SetActive(false);
        else LeftButton.SetActive(true);
        if(currentSelectedStage < StagePanels.Count - 1) RightButton.SetActive(true);
        else RightButton.SetActive(false);
        
        foreach (RectTransform rectPanels in StagePanelsRectTransforms)
        {
            stagePanelSequence.Join(rectPanels.DOMove(new Vector2(rectPanels.transform.position.x + StagePanelGap, rectPanels.transform.position.y), StagePanelMoveDuration));
        }
    }
    
    private void ResizeStagePanels()
    {
        foreach(GameObject panel in StagePanels)
        {
            if (StagePanels[currentSelectedStage] == panel) stagePanelSequence.Join(panel.GetComponent<RectTransform>().DOScale(new Vector2(1, 1), StagePanelMoveDuration));
            else stagePanelSequence.Join(panel.GetComponent<RectTransform>().DOScale(new Vector2(SmallStagePanelSizeX/BigStagePanelSizeX, SmallStagePanelSizeY/BigStagePanelSizeY), StagePanelMoveDuration));
        }
    }
    #endregion
    
    public void TitleButtonClicked()
    {
        if (stagePanelSequence == null || stagePanelSequence.IsPlaying()) return;
        StageSet.SetActive(false);
        TitlePanel.SetActive(true);
    }

    public void OptionButtonClicked()
    {
        if(stagePanelSequence == null || stagePanelSequence.IsPlaying()) return;
        OptionPanel.SetActive(true);
    }
    
    public void OnHoverEnter(BaseEventData data)
    {
        PointerEventData ped = (PointerEventData)data;
        GameObject hoveredObject = ped.pointerEnter;

        hoveredObject.GetComponent<RectTransform>().localScale =
            new Vector3(1.2f, 1.2f, 1f); // Scale up the hovered button
        
    }
    
    public void OnHoverExit(BaseEventData data)
    {
        initButtonSize();
    }

    private void initButtonSize()
    {
        TitleButton.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1f);
        OptionButton.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1f);
    }

}

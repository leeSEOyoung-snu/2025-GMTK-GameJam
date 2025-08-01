using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StageManager : MonoBehaviour
{
    #region Fields
    private int currentSelectedStage = 0;
    private string selectedStageFileName;
    
    Sequence stagePanelSequence;
    [SerializeField] private GameObject TitlePanel;
    [Header("Stage Panels Size")]
    [SerializeField] private float BigStagePanelSizeX;
    [SerializeField] private float BigStagePanelSizeY;
    [SerializeField] private float SmallStagePanelSizeX;
    [SerializeField] private float SmallStagePanelSizeY;
    [SerializeField] private float StagePanelGap;
    [SerializeField] private float StagePanelMoveDuration;
    [Header("Stage Panels Objects")]
    [SerializeField] private List<GameObject> StagePanels = new List<GameObject>();
    private List<RectTransform> StagePanelsRectTransforms = new List<RectTransform>();
    
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
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RightStageButtonClicekd();
                Debug.Log("Current Selected Panel: " + currentSelectedStage);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                LeftStageButtonClicekd();
                Debug.Log("Current Selected Panel: " + currentSelectedStage);
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
    
    public void BackButtonClicked()
    {
        gameObject.SetActive(false);
        TitlePanel.SetActive(true);
        
    }
    
}

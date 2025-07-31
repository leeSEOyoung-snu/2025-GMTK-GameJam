using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StageManager : MonoBehaviour
{
    #region Fields
    private int currentSelectedStage = 0;
    [Header("Stage Panels Size")]
    [SerializeField] private float BigStagePanelSizeX;
    [SerializeField] private float BigStagePanelSizeY;
    [SerializeField] private float SmallStagePanelSizeX;
    [SerializeField] private float SmallStagePanelSizeY;
    [SerializeField] private float StagePanelGap;
    [Header("Stage Panels Objects")]
    [SerializeField] private List<GameObject> StagePanels = new List<GameObject>();

    private List<RectTransform> StagePanelsRectTransforms = new List<RectTransform>();
    #endregion

    #region LifeCycle
    private void Awake()
    {
        foreach (var st in StagePanels)
        {
            StagePanelsRectTransforms.Add(st.GetComponent<RectTransform>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) RightStage(); ResizeStagePanels();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) LeftStage(); ResizeStagePanels();
    }

    #endregion

    private void ResizeStagePanels()
    {
        foreach(GameObject panel in StagePanels)
        {
            if (StagePanels[currentSelectedStage] == panel) panel.GetComponent<RectTransform>().DOSizeDelta(new Vector2(BigStagePanelSizeX, BigStagePanelSizeY), 1.0f);
            else panel.GetComponent<RectTransform>().DOSizeDelta(new Vector2(SmallStagePanelSizeX, SmallStagePanelSizeY), 1.0f);
        }
    }

    #region Methods
    public void RightStage()
    {
        currentSelectedStage++;
        
        if (currentSelectedStage > StagePanels.Count - 1)
        {
            currentSelectedStage = StagePanels.Count - 1;
            return;
        }
        
        foreach (RectTransform rectPanels in StagePanelsRectTransforms)
        {
            rectPanels.DOMove(new Vector2(rectPanels.transform.position.x - StagePanelGap, rectPanels.transform.position.y), 1.0f);
        }
        Debug.Log("Current Selected Stage: " + currentSelectedStage);
    }
    public void LeftStage()
    {
        currentSelectedStage--;
        if (currentSelectedStage < 0)
        {
            currentSelectedStage = 0;
            return;
        }
        foreach (RectTransform rectPanels in StagePanelsRectTransforms)
        {
            rectPanels.DOMove(new Vector2(rectPanels.transform.position.x + StagePanelGap, rectPanels.transform.position.y), 1.0f);
        }
        Debug.Log("Current Selected Stage: " + currentSelectedStage);

    }
    
    #endregion
    
    
}

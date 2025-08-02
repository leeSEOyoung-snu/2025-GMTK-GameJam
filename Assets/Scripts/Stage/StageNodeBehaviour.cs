using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public enum StageState
{
    Clear,
    Open,
    Closed
}
public class StageNodeBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject StageImage;
    [SerializeField] private TextMeshProUGUI StageText;

    private StageState stageState = StageState.Closed; // Default stage state
    private int BigStage;
    private int SmallStage;
    // Start is called before the first frame update
    void Start()
    {
        string numberStr = this.name.Substring("StageNode".Length); // "1010"
        int zeroIndex = numberStr.IndexOf('0');
        if (zeroIndex > 0 && zeroIndex < numberStr.Length - 1)
        {            // "010" → "1" in this case
            BigStage = int.Parse(numberStr.Substring(0, zeroIndex));
            SmallStage = int.Parse(numberStr.Substring(zeroIndex + 1));
            StageText.text = $"{BigStage} - {SmallStage}";
        }
        else
        {
            Debug.LogError("Invalid stage name format");
        }
        
        // Set the stage image based on the stage state
        int nodeData = GetComponentInParent<StagePanelBehaviour>().StageNodesData[SmallStage - 1];
        switch (nodeData)
        {
            case 0: //Closed
                stageState = StageState.Closed;
                StageImage.GetComponent<UnityEngine.UI.Image>().sprite = StageManager.Instance.ClosedIcon;
                break;
            case 1: //Open
                stageState = StageState.Open;
                StageImage.GetComponent<UnityEngine.UI.Image>().sprite = StageManager.Instance.OpenIcon;
                break;
            case 2: //Clear
                stageState = StageState.Clear;
                StageImage.GetComponent<UnityEngine.UI.Image>().sprite = StageManager.Instance.ClearIcon;
                break;
            default:
                Debug.LogError("Jot");
                break;
        }

    }

    // Update is called once per frame
    
    
    public void OnHoverEnter(BaseEventData data)
    {
        if(stageState == StageState.Open){
            this.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1f); // Scale up the node
        }
    }
    public void OnHoverExit(BaseEventData data)
    {
        if (stageState == StageState.Open)
        {
            this.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1f); // Reset scale
        }
    }

    public void OnClicked()
    {
        if (stageState == StageState.Open)
        {
            GameManager.Instance.SetStageData(BigStage, SmallStage);
        }
    }
}

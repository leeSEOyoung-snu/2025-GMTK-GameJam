using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public enum StageStage
{
    Clear,
    Open,
    Closed
}
public class StageNodeBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject StageImage;
    [SerializeField] private TextMeshProUGUI StageText;

    private StageStage stageStage = StageStage.Closed; // Default stage state
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
        
        
    }

    // Update is called once per frame
    
    
    public void OnHoverEnter(BaseEventData data)
    {
        this.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1f); // Scale up the node
    }
    public void OnHoverExit(BaseEventData data)
    {
        this.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1f); // Reset scale
    }

    public void OnClicked()
    {
        GameManager.Instance.SetStageData(BigStage, SmallStage);
    }
}

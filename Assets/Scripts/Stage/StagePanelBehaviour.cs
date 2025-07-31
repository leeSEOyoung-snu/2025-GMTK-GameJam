using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;

public class StagePanelBehaviour : MonoBehaviour
{
    [SerializeField] private List<GameObject> StageNodes;
    private string selectedStageFileName;
    private int currentSelectedNode = -1;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var sn in StageNodes)
        {
            EventTrigger trigger = sn.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = sn.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener(StageNodeClicked);
            
            trigger.triggers.Add(entry);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightStageButtonClicekd();
            Debug.Log("Current Selected Node: " + currentSelectedNode);
        }
        if( Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftStageButtonClicekd();
            Debug.Log("Current Selected Node: " + currentSelectedNode);
        }
    }
    
    private void StageNodeClicked(BaseEventData data)
    {
        PointerEventData ped = (PointerEventData)data;
        currentSelectedNode = StageNodes.IndexOf(ped.pointerPress);
        selectedStageFileName = transform.name + "_" + currentSelectedNode.ToString() + ".csv"; // Example file naming convention
        Debug.Log("Selected Stage File Name: " + selectedStageFileName);
        //TODO : connect to game manager and load the stage
        //TODO : file name convention
    }
    
    private void RightStageButtonClicekd()
    {
        currentSelectedNode++;
        if (currentSelectedNode >= StageNodes.Count) currentSelectedNode = StageNodes.Count - 1;
    }
    private void LeftStageButtonClicekd()
    {
        currentSelectedNode--;
        if (currentSelectedNode < 0) currentSelectedNode = 0;
    }
    
    
    
    //
}

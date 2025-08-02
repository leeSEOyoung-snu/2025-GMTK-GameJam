using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    [SerializeField] private GameObject tooltip;
    private RectTransform tooltipRectTransform;
    private TextMeshProUGUI tooltipText;
    // Start is called before the first frame update

    // Update is called once per frame
    private void Awake()
    {
        tooltipText = tooltip.GetComponentInChildren<TextMeshProUGUI>();
        tooltipRectTransform = tooltip.GetComponent<RectTransform>();
        setupTooltip("Tlqkf");  //for testing purposes
    }

    void Update()
    {
        //For test
        tooltipRectTransform.transform.position = Input.mousePosition;
    }

    public void setupTooltip(string text)
    {
        tooltipText.text = text;
        tooltipText.ForceMeshUpdate();  // Force the text to update its mesh
        tooltip.GetComponent<RectTransform>().sizeDelta = tooltipText.GetPreferredValues();
        //TODO : tooltip position should be fixed 
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    //Singleton instance
    public static TooltipManager Instance { get; private set; }
    [SerializeField] private GameObject tooltip;
    private RectTransform tooltipRectTransform;
    private TextMeshProUGUI tooltipText;
    // Start is called before the first frame update

    // Update is called once per frame
    private void Awake()
    {
        //Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        
        tooltipText = tooltip.GetComponentInChildren<TextMeshProUGUI>();
        tooltipRectTransform = tooltip.GetComponent<RectTransform>();
        HideTooltip();
    }

    void Update()
    {
        //For test
    }

    public void ShowTooltip()
    {
        tooltip.SetActive(true);
    }
    
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
    public void setupTooltip(string text, Vector2 tooltipposition)
    {
        tooltipText.text = text;
        tooltipText.ForceMeshUpdate();  // Force the text to update its mesh
        tooltip.GetComponent<RectTransform>().sizeDelta = tooltipText.GetPreferredValues();
        
        tooltipRectTransform.transform.position = tooltipposition;
    }
    
    
}

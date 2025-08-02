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
    private Dictionary<ResultTypes, string> resulTypeTooltip = new Dictionary<ResultTypes, string>
    {
        { ResultTypes.GenerateCard1, "Generate Card 1" },
        { ResultTypes.GenerateCard2, "Generate Card 2" },
        { ResultTypes.GenerateSushi, "Generate Sushi" },
        { ResultTypes.GiveTip, "Give Tip" },
        { ResultTypes.EmptyNextDish, "Empty Next Dish" },
        { ResultTypes.EmptyColorDish, "Empty Color Dish" },
        { ResultTypes.GenerateSushiOnColorDish, "Generate Sushi on Color Dish" },
        { ResultTypes.ChangeType, "Change Type" },
        { ResultTypes.ChangeCard, "Change Card" }
    };
    
    private Dictionary<ConditionTypes, string> conditionTooltip = new Dictionary<ConditionTypes, string>
    {
        { ConditionTypes.SushiEaten, "Sushi Eaten" },
        { ConditionTypes.CardPlaced, "Card Placed" },
        { ConditionTypes.CardGenerated, "Card Generated" },
        { ConditionTypes.SushiGenerated, "Sushi Generated" },
        { ConditionTypes.SushiPassed, "Sushi Passed" },
        { ConditionTypes.DishPassed, "Dish Passed" }
    };
    
    
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

    public void setupTooltip(ResultTypes resultTypes, Vector2 tooltipposition)
    {
        tooltipText.text = resulTypeTooltip[resultTypes];
        tooltipText.ForceMeshUpdate();  // Force the text to update its mesh
        tooltipRectTransform.sizeDelta = tooltipText.GetPreferredValues();
        tooltipRectTransform.transform.position = tooltipposition;
    }
    
    public void setupTooltip(ConditionTypes conditionTypes, Vector2 tooltipposition)
    {
        tooltipText.text = conditionTooltip[conditionTypes];
        tooltipText.ForceMeshUpdate();  // Force the text to update its mesh
        tooltipRectTransform.sizeDelta = tooltipText.GetPreferredValues();
        tooltipRectTransform.transform.position = tooltipposition;
    }
    
}

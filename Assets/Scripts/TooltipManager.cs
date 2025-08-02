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
        { ResultTypes.GenerateCard1, "Gain this card." },
        { ResultTypes.GenerateCard2, "Gain two copies of this card." },
        { ResultTypes.GenerateSushi, "Generate this sushi on the first empty plate." },
        { ResultTypes.GiveTip, "Pay double the price." },
        { ResultTypes.EmptyNextDish, "Turn the back plate into an empty plate." },
        { ResultTypes.EmptyColorDish, "Turn a plate of this color into an empty plate." },
        { ResultTypes.GenerateSushiOnColorDish, "Generate this sushi on a plate of this color." },
        { ResultTypes.ChangeType, "Turn the first sushi plate into the second sushi plate." },
        { ResultTypes.ChangeCard, "Turn the first card into the second card." }
    };
    
    private Dictionary<ConditionTypes, string> conditionTooltip = new Dictionary<ConditionTypes, string>
    {
        { ConditionTypes.SushiEaten, "If the cat eats this sushi…" },
        { ConditionTypes.CardPlaced, "If this card is used…" },
        { ConditionTypes.CardGenerated, "If this card is gained…" },
        { ConditionTypes.SushiGenerated, "If this sushi is generated…" },
        { ConditionTypes.SushiPassed, "If this sushi/empty plate passes by the cat…" },
        { ConditionTypes.DishPassed, "If a plate of this color passes by the cat…" }
    };
    
    
    // Update is called once per frame
    private void Awake()
    {
        //Singleton pattern
        if (Instance == null)
        {
            Instance = this;
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

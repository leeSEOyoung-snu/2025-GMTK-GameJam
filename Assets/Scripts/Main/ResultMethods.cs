using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResultTypes
{
    GenerateCard1,
    GenerateCard2,
    GenerateSushi,
    GiveTip,
    EmptyNextDish,
    EmptyColorDish,
    GenerateSushiOnColorDish,
    ChangeType,
    ChangeCard,
}

public class ResultMethods : MonoBehaviour
{
    public static ResultMethods Instance { get; private set; }
    
    public List<Sprite> iconSprites;
    

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    
    public void ActivateResult(ResultTypes type, bool isResultSingle, string resVal1, string resVal2, bool isVal1Sushi, bool isVal2Sushi)
    {
        SushiTypes sushi1 = SushiTypes.Any, sushi2 = SushiTypes.Any;
        ColorTypes dish1 = ColorTypes.DishStandBy, dish2 = ColorTypes.DishStandBy;

        if (isVal1Sushi)
        {
            if (Enum.TryParse(resVal1, ignoreCase: true, out sushi1)) { }
            else Debug.LogError($"Result Value1 Error: {resVal1}");
        }
        else
        {
            if (Enum.TryParse(resVal2, ignoreCase: true, out dish1)) { }
            else Debug.LogError($"Result Value2 Error: {resVal1}");
        }

        if (!isResultSingle)
        {
            if (isVal2Sushi)
            {
                if (Enum.TryParse(resVal1, ignoreCase: true, out sushi2)) { }
                else Debug.LogError($"Result Value1 Error: {resVal2}");
            }
            else
            {
                if (Enum.TryParse(resVal2, ignoreCase: true, out dish2)) { }
                else Debug.LogError($"Result Value2 Error: {resVal2}");
            }
        }
        
        switch (type)
        {
            case ResultTypes.GenerateCard1:
                CardManager.Instance.AddCard(sushi1, true);
                break;
            case ResultTypes.GenerateCard2:
                //TODO : Will be implemneted by SEO later
                break;
            case ResultTypes.GenerateSushi:
                break;
            case ResultTypes.GiveTip:
                break;
            case ResultTypes.EmptyNextDish:
                break;
            case ResultTypes.EmptyColorDish:
                break;
            case ResultTypes.GenerateSushiOnColorDish:
                break;
            case ResultTypes.ChangeType:
                break;
            case ResultTypes.ChangeCard:
                break;
        }
    }
    
}

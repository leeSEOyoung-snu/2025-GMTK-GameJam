using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    public Sprite singleBlank;
    public Sprite[] doubleBlank;
    public Sprite arrow;
    public GameObject anyPref;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    
    public void ActivateResult(ResultTypes type, bool isResultSingle, string resVal1, string resVal2, bool isVal1Sushi, bool isVal2Sushi)
    {
        SushiTypes sushi1 = SushiTypes.Any, sushi2 = SushiTypes.Any;
        ColorTypes dish1 = ColorTypes.DishStandBy, dish2 = ColorTypes.DishStandBy;

        if (type == ResultTypes.GiveTip) return;

        if (type == ResultTypes.EmptyNextDish)
        {
            TableManager.Instance.EmptyNextDish(int.Parse(resVal1));
            return;
        }
        else if (type == ResultTypes.EmptyColorDish)
        {
            TableManager.Instance.EmptyNextDish(Enum.Parse<ColorTypes>(resVal1, true), int.Parse(resVal2));
            return;
        }
            
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
                CardManager.Instance.AddCard(sushi1, true);
                CardManager.Instance.AddCard(sushi2, sushi1 != sushi2);
                break;
            
            case ResultTypes.GenerateSushi:
                TableManager.Instance.GenerateSushi(sushi1);
                break;
            
            case ResultTypes.EmptyColorDish:
                // TODO: import result
                break;
            
            case ResultTypes.GenerateSushiOnColorDish:
                TableManager.Instance.GenerateSushi(sushi1, dish2);
                break;
            
            case ResultTypes.ChangeType:
                if (isVal1Sushi && isVal2Sushi)
                    TableManager.Instance.ChangeType(sushi1, sushi2);
                else
                    Debug.LogError($"Change Type Error: {resVal1}, {resVal2}");
                break;
            
            case ResultTypes.ChangeCard:
                // TODO: import result
                break;
        }
    }
    
}

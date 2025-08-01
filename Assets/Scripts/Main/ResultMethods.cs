using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResultTypes
{
    GenerateCard,
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
    
    public void ActivateResult(ResultTypes type, object resVal1, object resVal2)
    {
        switch (type)
        {
            case ResultTypes.GenerateCard:
                SushiTypes sushi1, sushi2;
                if (Enum.TryParse((string)resVal1, ignoreCase: true, out sushi1))
                {
                    if (Enum.TryParse((string)resVal2, ignoreCase: true, out sushi2))
                    {
                        // GenerateCard(sushi1, sushi2);
                    }
                    else GenerateCard(sushi1);
                }
                else Debug.LogError($"GenerateCard Value Error: {resVal1}");
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
    
    
    
    #region GenerateCard

    public void GenerateCard(SushiTypes sushiType)
    {
        CardManager.Instance.AddCard(sushiType);
    }
    
    #endregion
}

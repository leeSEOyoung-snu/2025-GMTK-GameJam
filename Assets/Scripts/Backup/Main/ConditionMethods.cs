using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionTypes
{
    SushiEaten,
    CardPlaced,
    CardGenerated,
    SushiGenerated,
    SushiPassed,
    DishPassed,
    Empty
}

public class ConditionMethods : MonoBehaviour
{
    public static ConditionMethods Instance { get; private set; }
    
    public List<Sprite> iconSprites;
    public List<Sprite> sushiSprites;
    public List<Sprite> dishSprites;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public (Sprite[], bool, bool) GetConditionSprites(ConditionTypes type, object conval)
    {
        // [icon, type], isStandBy, isSushiType
        Sprite[] sprites = new Sprite[2];
        bool isStandBy = false, isSushiType = true;

        if (Enum.TryParse((string)conval, ignoreCase: true, out SushiTypes sushi))
        {
            switch (type)
            {
                case ConditionTypes.SushiEaten:     sprites = SushiEaten(sushi);     break;
                case ConditionTypes.CardGenerated:  sprites = CardGenerated(sushi);  break;
                case ConditionTypes.CardPlaced:     sprites = CardPlaced(sushi);     break;
                case ConditionTypes.SushiGenerated: sprites = SushiGenerated(sushi); break;
                case ConditionTypes.SushiPassed:    sprites = SushiPassed(sushi);    break;
            }

            isStandBy = sushi == SushiTypes.SushiStandBy;
        }
        else if (Enum.TryParse((string)conval, ignoreCase: true, out ColorTypes color))
        {
            switch (type)
            {
                case ConditionTypes.DishPassed: sprites = DishPassed(color); break;
            }

            Debug.Log(sprites.Length);
            isSushiType = false;
            isStandBy = color == ColorTypes.DishStandBy;
        }

        return (sprites, isStandBy, isSushiType);
    }
    

    // 0
    #region SushiEaten
    
    public Sprite[] SushiEaten(SushiTypes condition)
    {
        Sprite[] result = new Sprite[2];
        result[0] = iconSprites[0];
        result[1] = sushiSprites[(int)condition];
        return result;
    }
    
    public bool SushiEaten(SushiTypes condition, SushiTypes eaten)
    {
        return condition == eaten;
    }
    
    #endregion

    // 1
    #region CardPlaced

    public Sprite[] CardPlaced(SushiTypes condition)
    {
        Sprite[] result = new Sprite[2];
        result[0] = iconSprites[1];
        result[1] = sushiSprites[(int)condition];
        return result;
    }
    
    public bool CardPlaced(SushiTypes condition, SushiTypes placed)
    {
        return condition == placed;
    }
    
    #endregion

    // 2
    #region CardGenerated

    public Sprite[] CardGenerated(SushiTypes condition)
    {
        Sprite[] result = new Sprite[2];
        result[0] = iconSprites[2];
        result[1] = sushiSprites[(int)condition];
        return result;
    }
    
    public bool CardGenerated(SushiTypes condition, SushiTypes generated)
    {
        return condition == generated;
    }
    
    #endregion

    // 3
    #region SushiGenerated

    public Sprite[] SushiGenerated(SushiTypes condition)
    {
        Sprite[] result = new Sprite[2];
        result[0] = iconSprites[3];
        result[1] = sushiSprites[(int)condition];
        return result;
    }
    
    public bool SushiGenerated(SushiTypes condition, SushiTypes generated)
    {
        return condition == generated;
    }
    
    #endregion

    // 4
    #region SushiPassed

    public Sprite[] SushiPassed(SushiTypes condition)
    {
        Sprite[] result = new Sprite[2];
        result[0] = iconSprites[4];
        result[1] = sushiSprites[(int)condition];
        return result;
    }
    
    public bool SushiPassed(SushiTypes condition, SushiTypes passed)
    {
        return condition == passed;
    }
    
    #endregion

    // 5
    # region DishPassed

    public Sprite[] DishPassed(ColorTypes condition)
    {
        Sprite[] result = new Sprite[2];
        result[0] = iconSprites[5];
        result[1] = dishSprites[(int)condition];
        return result;
    }
    
    public bool DishPassed(ColorTypes condition, ColorTypes passed)
    {
        return condition == passed;
    }
    
    #endregion
}

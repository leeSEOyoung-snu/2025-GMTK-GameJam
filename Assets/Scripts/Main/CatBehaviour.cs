using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CatBehaviour : MonoBehaviour
{
    [SerializeField] private SpriteRenderer catSr;
    [SerializeField] private CatConditionBehaviour catConditionBehaviour;
    
    private readonly float _sushiTypeScale, _dishTypeScale;
    private ColorTypes color;

    private ConditionTypes conditionType;
    public string Condition { private get; set; }

    public void InitCat(Vector3 initPos, Dictionary<string, object> catData)
    {
        transform.localPosition = initPos;
        
        catSr.sprite = DiningManager.Instance.catSprites[(int)catData["Sprite"]];
        if (Enum.TryParse((string)catData["Color"], ignoreCase: true, out color))
        {
            // TODO: Cat 색상 지정 필요
            switch (color)
            {
                case ColorTypes.W: catSr.color = Color.white; break;
                case ColorTypes.R: catSr.color = Color.red; break;
                case ColorTypes.Y: catSr.color = Color.yellow; break;
                case ColorTypes.B: catSr.color = Color.blue; break;
            }
        }
        else Debug.LogError("Cat Color Error: " + (string)catData["Color"]);

        Condition = (string)catData["Condition"];

        if (Enum.TryParse(Condition, ignoreCase: true, out conditionType))
        {
            var conditionData =
                ConditionMethods.Instance.GetConditionSprites(conditionType, catData["ConVal1"]);
            catConditionBehaviour.InitCondition(conditionData.Item1, conditionData.Item2, conditionData.Item3);
        }
        else Debug.LogError("Condition Error: " + Condition);
    }
}

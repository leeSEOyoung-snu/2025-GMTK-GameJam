using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatResultBehaviour : IPointerHandler
{
    [SerializeField] private CatBehaviour catBehaviour;
    [SerializeField] private SpriteRenderer iconSr;
    [SerializeField] private GameObject singleResultPref;
    [SerializeField] private GameObject doubleResultPref;
    [SerializeField] private GameObject pointerDetector;
    [SerializeField] private GameObject arrow;

    private GameObject result;
    private bool isResultSingle, isVal1StandBy, isVal2StandBy;
    private Transform result1Tr, result2Tr;
    private bool isVal1Sushi, isVal2Sushi;

    public void InitResult(bool isVal1Sushi, bool isVal2Sushi, bool isVal1StandBy, bool isVal2StandBy, ResultTypes resultType, string val1Str, string val2Str)
    {
        SushiTypes sushi1, sushi2;
        ColorTypes dish1, dish2;
        arrow.SetActive(false);
        switch (resultType)
        {
            case ResultTypes.GenerateCard1: case ResultTypes.GenerateSushi:
                isResultSingle = true;
                result = Instantiate(singleResultPref, transform);
                result1Tr = result.transform;
                iconSr.sprite = ResultMethods.Instance.iconSprites[(int)resultType];
                sushi1 = Enum.Parse<SushiTypes>(val1Str, true);
                if (sushi1 == SushiTypes.SushiStandBy)
                {
                    result.GetComponent<SpriteRenderer>().sprite = ResultMethods.Instance.singleBlank;
                    result.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    result.GetComponent<SpriteRenderer>().sprite = ConditionMethods.Instance.sushiSprites[(int)sushi1];
                    result.transform.localScale = new Vector3(DiningManager.Instance.BubbleSushiScale,
                        DiningManager.Instance.BubbleSushiScale, 0);
                }
                result1Tr.transform.localPosition = new Vector3(DiningManager.Instance.BubbleTypePosX, 0, 0);
                result.transform.SetAsFirstSibling();
                break;
            
            case ResultTypes.GenerateCard2:
                isResultSingle = false;
                result = Instantiate(doubleResultPref, transform);
                iconSr.sprite = ResultMethods.Instance.iconSprites[(int)resultType];
                sushi1 = Enum.Parse<SushiTypes>(val1Str, true);
                sushi2 = Enum.Parse<SushiTypes>(val2Str, true);
                result1Tr = result.transform.Find("Result1");
                result2Tr = result.transform.Find("Result2");
                if (sushi1 == SushiTypes.SushiStandBy)
                {
                    result1Tr.GetComponent<SpriteRenderer>().sprite = ResultMethods.Instance.doubleBlank[1];
                    result1Tr.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    result1Tr.GetComponent<SpriteRenderer>().sprite = ConditionMethods.Instance.sushiSprites[(int)sushi1];
                    result1Tr.localScale = new Vector3(DiningManager.Instance.BubbleSushiScale,
                        DiningManager.Instance.BubbleSushiScale, 0);
                }

                if (sushi2 == SushiTypes.SushiStandBy)
                {
                    result2Tr.GetComponent<SpriteRenderer>().sprite = ResultMethods.Instance.doubleBlank[1];
                    result2Tr.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    result2Tr.GetComponent<SpriteRenderer>().sprite = ConditionMethods.Instance.sushiSprites[(int)sushi2];
                    result2Tr.localScale = new Vector3(DiningManager.Instance.BubbleSushiScale,
                        DiningManager.Instance.BubbleSushiScale, 0);
                }
                result.transform.localPosition = new Vector3(DiningManager.Instance.BubbleTypePosX, 0, 0);
                result.transform.SetAsFirstSibling();
                break;
            
            case ResultTypes.GiveTip: case ResultTypes.EmptyNextDish:
                isResultSingle = true;
                iconSr.sprite = ResultMethods.Instance.iconSprites[(int)resultType];
                iconSr.transform.localPosition = new Vector3(0, 0, 0);
                break;
            
            case ResultTypes.EmptyColorDish:
                arrow.SetActive(true);
                isResultSingle = true;
                result = Instantiate(singleResultPref, transform);
                result1Tr = iconSr.transform;
                result2Tr = result.transform;
                
                dish1 = Enum.Parse<ColorTypes>(val1Str, true);
                iconSr.sprite = TableManager.Instance.dishSprites[(int)dish1];
                iconSr.transform.localScale = new Vector3(DiningManager.Instance.BubbleDishScale,
                    DiningManager.Instance.BubbleDishScale, 0);
                Instantiate(ResultMethods.Instance.anyPref, result1Tr);
                
                result2Tr.GetComponent<SpriteRenderer>().sprite = ResultMethods.Instance.iconSprites[(int)dish1];
                result2Tr.localScale = new Vector3(DiningManager.Instance.BubbleDishScale,
                    DiningManager.Instance.BubbleDishScale, 0);
                result2Tr.transform.localPosition = new Vector3(DiningManager.Instance.BubbleTypePosX, 0, 0);
                result.transform.SetAsFirstSibling();
                break;
            
            case ResultTypes.GenerateSushiOnColorDish:
                break;
            case ResultTypes.ChangeType:
                break;
            case ResultTypes.ChangeCard:
                break;
            
            
            
            // GenerateSushiOnColorDish,
            // ChangeType,
            // ChangeCard,
        }
        
        this.isVal1StandBy = isVal1Sushi;
        this.isVal2StandBy = isVal2Sushi;
        this.isVal1Sushi = isVal1Sushi;
        this.isVal2Sushi = isVal2Sushi;
        pointerDetector.SetActive(true);
    }

    public override void HandlePointerClick()
    {
        Debug.Log($"isVal1StandBy: {isVal1StandBy}, isVal2StandBy: {isVal2StandBy}");
        if (MainSceneManager.Instance.CookStarted || !(isVal1StandBy || isVal2StandBy)) return;
        CardManager.Instance.ResultSelected(this);
    }

    public void SetResult(SushiTypes sushi)
    {
        float scaleFactor;
        if (isResultSingle)
        {
            isVal1StandBy = isVal2StandBy = false;
            result1Tr.GetComponent<SpriteRenderer>().sprite = TableManager.Instance.sushiSprites[(int)sushi];
            result1Tr.localScale = new Vector3(DiningManager.Instance.BubbleSushiScale, DiningManager.Instance.BubbleSushiScale, 0);
            catBehaviour.Result1 = sushi.ToString();
        }
        else if (isVal1StandBy)
        {
            isVal1StandBy = false;
            result1Tr.GetComponent<SpriteRenderer>().sprite = TableManager.Instance.sushiSprites[(int)sushi];
            result1Tr.localScale = new Vector3(DiningManager.Instance.BubbleSushiScale, DiningManager.Instance.BubbleSushiScale, 0);
            catBehaviour.Result1 = sushi.ToString();
        }
        else
        {
            isVal2StandBy = false;
            result2Tr.GetComponent<SpriteRenderer>().sprite = TableManager.Instance.sushiSprites[(int)sushi];
            result2Tr.localScale = new Vector3(DiningManager.Instance.BubbleSushiScale, DiningManager.Instance.BubbleSushiScale, 0);
            catBehaviour.Result2 = sushi.ToString();
        }
    }

    public override void HandlePointerEnter()
    {
        TooltipManager.Instance.ShowTooltip();
        TooltipManager.Instance.setupTooltip(catBehaviour.resultType, Camera.main.WorldToScreenPoint(this.transform.position)+new Vector3(0,-75f,0));
    }

    public override void HandlePointerExit()
    {
        TooltipManager.Instance.HideTooltip();
    }
}

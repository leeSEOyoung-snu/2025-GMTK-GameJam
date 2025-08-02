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

    private GameObject result;
    private bool isResultSingle, isVal1StandBy, isVal2StandBy;

    public void InitResult(Sprite[] sprites, bool isVal1Sushi, bool isVal2Sushi, bool isVal1StandBy, bool isVal2StandBy)
    {
        iconSr.sprite = sprites[0];

        if (sprites.Length == 2)
        {
            isResultSingle = true;
            result = Instantiate(singleResultPref, transform);
            if (isVal1Sushi) result.transform.localScale = new Vector3(DiningManager.Instance.BubbleSushiScale, DiningManager.Instance.BubbleSushiScale, 0);
            else result.transform.localScale = new Vector3(DiningManager.Instance.BubbleDishScale, DiningManager.Instance.BubbleDishScale, 0);
            result.GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
        else
        {
            isResultSingle = false;
            result = Instantiate(doubleResultPref, transform);
            Transform result1Tr = transform.Find("Result1"), result2Tr = transform.Find("Result2");
            if (isVal1Sushi) result1Tr.localScale = new Vector3(DiningManager.Instance.BubbleSushiScale, DiningManager.Instance.BubbleSushiScale, 0);
            else result1Tr.localScale = new Vector3(DiningManager.Instance.BubbleDishScale, DiningManager.Instance.BubbleDishScale, 0);
            result1Tr.GetComponent<SpriteRenderer>().sprite = sprites[1];
            if (isVal2Sushi) result2Tr.localScale = new Vector3(DiningManager.Instance.BubbleSushiScale, DiningManager.Instance.BubbleSushiScale, 0);
            else result2Tr.localScale = new Vector3(DiningManager.Instance.BubbleDishScale, DiningManager.Instance.BubbleDishScale, 0);
            result2Tr.GetComponent<SpriteRenderer>().sprite = sprites[2];
        }
        
        result.transform.SetAsFirstSibling();
        result.transform.localPosition = new Vector3(DiningManager.Instance.BubbleTypePosX, 0, 0);
        
        this.isVal1StandBy = isVal1Sushi;
        this.isVal2StandBy = isVal2Sushi;
        pointerDetector.SetActive(isVal1StandBy || isVal2StandBy);
    }

    public override void HandlePointerClick()
    {
        if (MainSceneManager.Instance.CookStarted || !(isVal1StandBy || isVal2StandBy)) return;
        CardManager.Instance.ResultSelected(this);
    }

    public void SetResult(SushiTypes sushi)
    {
        if (isResultSingle)
        {
            pointerDetector.SetActive(false);
            isVal1StandBy = isVal2StandBy = false;
            result.GetComponent<SpriteRenderer>().sprite = TableManager.Instance.sushiSprites[(int)sushi];
            catBehaviour.Result1 = sushi.ToString();
        }
        else if (isVal1StandBy)
        {
            if (!isVal2StandBy) pointerDetector.SetActive(false);
            isVal1StandBy = false;
            result.transform.Find("Result1").GetComponent<SpriteRenderer>().sprite = TableManager.Instance.sushiSprites[(int)sushi];
            catBehaviour.Result1 = sushi.ToString();
        }
        else
        {
            pointerDetector.SetActive(false);
            isVal2StandBy = false;
            result.transform.Find("Result2").GetComponent<SpriteRenderer>().sprite = TableManager.Instance.sushiSprites[(int)sushi];
            catBehaviour.Result2 = sushi.ToString();
        }
    }

    public override void HandlePointerEnter() { }

    public override void HandlePointerExit() { }
}

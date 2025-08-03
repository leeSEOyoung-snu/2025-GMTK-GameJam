using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CatBehaviour : MonoBehaviour
{
    [SerializeField] private SpriteRenderer catSr;
    [SerializeField] private CatConditionBehaviour catConditionBehaviour;
    [SerializeField] private CatResultBehaviour catResultBehaviour;
    [SerializeField] private SpriteRenderer priceSr;
    [SerializeField] private GameObject bubble;
    
    private readonly float _sushiTypeScale, _dishTypeScale;
    private ColorTypes color;

    private readonly float _tmpAnimationFactor = 0.5f;
    private Sequence _catSeq;

    private int id;
    

    public ConditionTypes conditionType { get; private set; }
    public string Condition;

    public ResultTypes resultType;
    private bool isResultSingle, isResult1Sushi, isResult2Sushi;
    public string Result1;
    public string Result2;

    public bool isFull;

    public void InitCat(Vector3 initPos, Dictionary<string, object> catData, int id)
    {
        this.id = id;
        
        priceSr.gameObject.SetActive(false);
        
        transform.localPosition = initPos;
        
        isFull = false;
        
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
        
        
        // Generate Condition
        if (Enum.TryParse((string)catData["Condition"], ignoreCase: true, out ConditionTypes cond))
        {
            Condition = (string)catData["ConVal1"];
            conditionType = cond;
            var conditionData =
                ConditionMethods.Instance.GetConditionSprites(conditionType, catData["ConVal1"]);
            catConditionBehaviour.InitCondition(conditionData.Item1, conditionData.Item2, conditionData.Item3);
        }
        else if (string.Equals((string)catData["Condition"], "X", StringComparison.OrdinalIgnoreCase))
        {
            conditionType = ConditionTypes.Empty;
            bubble.SetActive(false);
            return;
        }
        else Debug.LogError("Condition Error: " + (string)catData["Condition"]);
        
        // Generate Result
        Result1 = (string)catData["ResVal1"];
        Result2 = (string)catData["ResVal2"];
        isResultSingle = Result2.Equals("X", StringComparison.OrdinalIgnoreCase);
        
        // Sprite[] sprites = isResultSingle ? new Sprite[2] : new Sprite[3];
        bool isVal1Sushi = false, isVal2Sushi = false, isVal1StandBy = false, isVal2StandBy = false;

        
        if (Enum.TryParse((string)catData["Result"], ignoreCase: true, out ResultTypes rt))
            this.resultType = rt;
        else Debug.LogError("Result Error: " + (string)catData["Result"]);

        if (Result1.Equals("X", StringComparison.OrdinalIgnoreCase))
        {
            isVal1Sushi = false;
            isVal1StandBy = false;
        }
        else if (Enum.TryParse(Result1, ignoreCase: true, out SushiTypes sushi1))
        {
            isVal1Sushi = true;
            // sprites[1] = ConditionMethods.Instance.sushiSprites[(int)sushi1];
            isVal1StandBy = sushi1 == SushiTypes.SushiStandBy;
        }
        else if (Enum.TryParse(Result1, ignoreCase: true, out ColorTypes dish1))
        {
            isVal1Sushi = false;
            // sprites[1] = ConditionMethods.Instance.dishSprites[(int)dish1];
            isVal1StandBy = dish1 == ColorTypes.DishStandBy;
        }
        else Debug.LogError("Result Val1 Error: " + Result1);

        if (!isResultSingle)
        {
            if (Enum.TryParse(Result1, ignoreCase: true, out SushiTypes sushi2))
            {
                isVal2Sushi = true;
                // sprites[2] = ConditionMethods.Instance.sushiSprites[(int)sushi2];
                isVal2StandBy = sushi2 == SushiTypes.SushiStandBy;
            }
            else if (Enum.TryParse(Result1, ignoreCase: true, out ColorTypes dish2))
            {
                isVal2Sushi = false;
                // sprites[2] = ConditionMethods.Instance.dishSprites[(int)dish2];
                isVal2StandBy = dish2 == ColorTypes.DishStandBy;
            }
            else Debug.LogError("Result Val2 Error: " + Result2);
        }

        isResult1Sushi = isVal1Sushi;
        isResult2Sushi = isVal2Sushi;
        Debug.LogError($"type: {resultType}, result1: {isResult1Sushi}, result2: {isResult2Sushi}");
        catResultBehaviour.InitResult(isVal1Sushi, isVal2Sushi, isVal1StandBy, isVal2StandBy, resultType, Result1, Result2);
    }

    public bool CheckCondition(ConditionTypes conditionType, string condition, bool isSushiEmpty = false)
    {
        if (conditionType == ConditionTypes.DishPassed
            && this.conditionType == conditionType
            && Condition == ColorTypes.DishEmpty.ToString()
            && isSushiEmpty) return true;
        else return this.conditionType == conditionType && Condition == condition;
    }

    public void ActivateResult()
    {
        if (conditionType == ConditionTypes.Empty) return;
        Debug.Log($"result: {resultType}");
        if (resultType == ResultTypes.EmptyNextDish) 
            ResultMethods.Instance.ActivateResult(resultType, isResultSingle, id.ToString(), Result2, isResult1Sushi, isResult2Sushi);
        else if (resultType == ResultTypes.GenerateSushiOnColorDish)
            ResultMethods.Instance.ActivateResult(resultType, false, Result1, Result2, false, true);
        else if ((resultType != ResultTypes.GiveTip))
            ResultMethods.Instance.ActivateResult(resultType, isResultSingle, Result1, Result2, isResult1Sushi, isResult2Sushi);
        // TODO: Condition 충족 시 애니메이션 수정
        if (_catSeq != null && _catSeq.IsActive() && _catSeq.IsPlaying())
        {
            _catSeq.Kill(true);
        }
        _catSeq = DOTween.Sequence();
        Vector3[] path = { new Vector3(transform.localPosition.x, transform.localPosition.y + _tmpAnimationFactor, transform.localPosition.z), transform.localPosition };
        _catSeq.Append(transform.DOLocalPath(path, _tmpAnimationFactor));
        _catSeq.Play().OnComplete(() => { InteractionManager.Instance.TriggerProcess(true); });
    }

    public bool TryEat(ColorTypes dishColor, DishBehaviour dish)
    {
        if (isFull)
        {
            InteractionManager.Instance.CheckRelativeCompleted();
            return false;
        }
        bool result;
        if (dish.DishData.Sushi == SushiTypes.SushiEmpty) result = false;
        else if (color == ColorTypes.W || dishColor == ColorTypes.W) result = true;
        else if (dishColor == color) result = true;
        else result = false;
        
        if (result)
        {
            isFull = true;
            EatMotion(dish);
        }
        else InteractionManager.Instance.CheckRelativeCompleted();
        return result;
    }

    private void EatMotion(DishBehaviour dish)
    {
        // TODO: 먹는 모션 수정
        if (_catSeq != null && _catSeq.IsActive() && _catSeq.IsPlaying())
        {
            _catSeq.Kill(true);
        }
        _catSeq = DOTween.Sequence();
        Vector3[] path = { new Vector3(transform.localPosition.x, transform.localPosition.y - _tmpAnimationFactor, transform.localPosition.z), transform.localPosition };
        _catSeq.Append(transform.DOLocalPath(path, _tmpAnimationFactor));
        _catSeq.Play().OnComplete(() => { InteractionManager.Instance.CheckRelativeCompleted(); });
    }

    public void ShowPrice(SushiTypes sushiType)
    {
        int price = MainSceneManager.Instance.Price[sushiType];
        if (resultType == ResultTypes.GiveTip) price *= 2;
        MainSceneManager.Instance.ChangeScore(price);
        StartCoroutine(PriceCoroutine(resultType == ResultTypes.GiveTip));
    }

    public IEnumerator PriceCoroutine(bool hasTip)
    {
        if (hasTip) priceSr.color = Color.red;
        else priceSr.color = Color.green;
        priceSr.gameObject.SetActive(true);
        yield return new WaitForSeconds(_tmpAnimationFactor);
        priceSr.gameObject.SetActive(false);
    }
}
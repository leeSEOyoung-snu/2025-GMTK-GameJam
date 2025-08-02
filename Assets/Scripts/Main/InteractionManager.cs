using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour, IInit
{
    public static InteractionManager Instance { get; private set; }

    public Dictionary<int, DishBehaviour> CatDishRelative;
    
    private List<int> satisfiedIds;
    private List<int> eatenList;

    private int currCompletedCondition;
    private int currCompletedRelative;
    private int currCompletedResult;

    public Queue<(ConditionTypes, string)> ConditionsQueue;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Init()
    {
        satisfiedIds = new List<int>();
        eatenList = new List<int>();
        CatDishRelative = new Dictionary<int, DishBehaviour>();
        currCompletedCondition = 0;
        currCompletedRelative = 0;
        currCompletedResult = 0;
        
        ConditionsQueue = new Queue<(ConditionTypes, string)>();
    }

    public void InitCatDishRelative()
    {
        CatDishRelative.Clear();
        for (int i = 0; i < DiningManager.Instance.CatCnt; i++)
        {
            CatDishRelative.Add(i, null);
        }
    }

    public void CheckCatDishRelative()
    {
        eatenList.Clear();
        currCompletedRelative = 0;
        foreach (var pair in CatDishRelative)
        {
            if (pair.Value == null)
            {
                currCompletedRelative++;
                continue;
            }
            DiningManager.Instance.CatBehaviourDict[pair.Key].TryEat(pair.Value.DishData.Color);
        }
    }

    public void CheckRelativeCompleted(bool eaten, int catId)
    {
        if (eaten)
        {
            eatenList.Add(catId);
        }
        if (currCompletedRelative < DiningManager.Instance.CatCnt - 1) { currCompletedRelative++; return; }

        currCompletedRelative = 0;
        
        for (int i = 0; i < eatenList.Count; i++)
        {
            var dish = CatDishRelative[eatenList[i]];
            CheckCondition(ConditionTypes.SushiEaten, dish.DishData.Sushi.ToString());
            dish.ChangeSushiType(SushiTypes.Empty);
        }
    }

    public void CheckCondition(ConditionTypes condition, string valStr)
    {
        satisfiedIds.Clear();
        currCompletedCondition = 0;
        for (int i = 0; i < DiningManager.Instance.CatCnt; i++)
        {
            DiningManager.Instance.CatBehaviourDict[i].CheckCondition(condition, valStr);
        }
    }

    public void CheckConditionCompleted(bool check, int id)
    {
        if (check)
        {
            satisfiedIds.Add(id);
        }
        if (currCompletedCondition < DiningManager.Instance.CatCnt - 1) { currCompletedCondition++; return; }
        
        currCompletedCondition = 0;

        if (satisfiedIds.Count == 0)
        {
            MainSceneManager.Instance.CheckConditionCompleted();
            return;
        }

        currCompletedResult = 0;
        foreach (int catId in satisfiedIds)
        {
            DiningManager.Instance.CatBehaviourDict[catId].ActivateResult();
        }
    }

    public void CheckResultCompleted()
    {
        Debug.Log("Ended");
        if (currCompletedResult < DiningManager.Instance.CatCnt - 1) { currCompletedResult++; return; }
        currCompletedResult = 0;
        MainSceneManager.Instance.CheckConditionCompleted();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour, IInit
{
    public static InteractionManager Instance { get; private set; }

    public Dictionary<int, DishBehaviour> CatDishRelative;
    
    private List<int> satisfiedIds;

    private int currCompletedCondition;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Init()
    {
        satisfiedIds = new List<int>();
        CatDishRelative = new Dictionary<int, DishBehaviour>();
        currCompletedCondition = 0;
    }

    public void InitCatDishRelative()
    {
        CatDishRelative = new Dictionary<int, DishBehaviour>();
        for (int i = 0; i < DiningManager.Instance.CatCnt; i++)
        {
            CatDishRelative.Add(i, null);
        }
    }

    public void CheckCatDishRelative()
    {
        foreach (var pair in CatDishRelative)
        {
            if (pair.Value == null) continue;
            bool eaten = DiningManager.Instance.CatBehaviourDict[pair.Key].TryEat(pair.Value.DishData.Color);
            if (eaten) pair.Value.ChangeSushiType(SushiTypes.Empty);
        }
        
        TableManager.Instance.RotateDishOnce();
    }

    public void CheckCondition(ConditionTypes condition, string valStr)
    {
        satisfiedIds = new List<int>();
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

        Debug.Log(satisfiedIds);
        currCompletedCondition = 0;

        if (satisfiedIds.Count == 0)
        {
            MainSceneManager.Instance.CheckConditionCompleted();
            return;
        }

        foreach (int catId in satisfiedIds)
        {
            DiningManager.Instance.CatBehaviourDict[catId].ActivateResult();
        }
    }
}

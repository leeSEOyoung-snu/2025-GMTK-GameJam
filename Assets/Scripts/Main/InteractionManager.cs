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

    public void CheckCondition(ConditionTypes condition, string valStr)
    {
        satisfiedIds = new List<int>();
        for (int i = 0; i < DiningManager.Instance.CatCnt; i++)
        {
            DiningManager.Instance.CatBehaviourDict[i].CheckCondition(condition, valStr);
        }
    }


    public void CheckConditionCompleted(bool check, int id)
    {
        if (check) satisfiedIds.Add(id);
        if (currCompletedCondition < DiningManager.Instance.CatCnt - 1) { currCompletedCondition++; return; }

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

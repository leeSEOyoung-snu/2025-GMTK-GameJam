using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionManager : MonoBehaviour, IInit
{
    public static InteractionManager Instance { get; private set; }

    public Dictionary<int, DishBehaviour> CatDishRelative;
    
    
    private List<int> eatenList;
    
    private int currCompletedRelative;

    private Queue<(ConditionTypes, string)> conditionQueue;
    private Queue<int> activateCatIdx;
    private (ConditionTypes, string) currentCondition;

    public bool isProcessing;
    
    private int currCompletedCondition;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Init()
    {
        eatenList = new List<int>();
        CatDishRelative = new Dictionary<int, DishBehaviour>();
        currCompletedRelative = 0;
        
        conditionQueue = new Queue<(ConditionTypes, string)>();
        activateCatIdx = new Queue<int>();
        isProcessing = false;
        
        currCompletedCondition = 0;
    }

    #region Check Condition
    public void EnQueueCondition(ConditionTypes conditionType, string val1)
    {
        conditionQueue.Enqueue((conditionType, val1));
    }

    public void TriggerProcess()
    {
        if (isProcessing || conditionQueue.Count == 0) return;
        isProcessing = true;
        currentCondition = conditionQueue.Dequeue();
        CheckCondition();
    }

    private void CheckCondition()
    {
        activateCatIdx.Clear();
        currCompletedCondition = 0;
        for (int i = 0; i < DiningManager.Instance.CatCnt; i++)
        {
            DiningManager.Instance.CatBehaviourDict[i].CheckCondition(currentCondition.Item1, currentCondition.Item2);
        }
    }

    public void CheckConditionCompleted(bool isSatisfied, int catId)
    {
        if (isSatisfied) { activateCatIdx.Enqueue(catId); }
        if (currCompletedCondition < DiningManager.Instance.CatCnt - 1) { currCompletedCondition++; return; }
        
        currCompletedCondition = 0;

        ActivateResult();
    }

    public void ActivateResult()
    {
        if (activateCatIdx.Count > 0)
        {
            DiningManager.Instance.CatBehaviourDict[activateCatIdx.Dequeue()].ActivateResult();
        }
        else
        {
            if (conditionQueue.Count == 0)
            {
                isProcessing = false;
                MainSceneManager.Instance.CheckConditionCompleted();
            }
            else
            {
                currentCondition = conditionQueue.Dequeue();
                CheckCondition();
            }
        }
    }
    
    #endregion
    
    #region Cat Dish Relative
    
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
            activateCatIdx.Enqueue(catId);
        }
        if (currCompletedRelative < DiningManager.Instance.CatCnt - 1) { currCompletedRelative++; return; }

        currCompletedRelative = 0;
        
        for (int i = 0; i < eatenList.Count; i++)
        {
            CatDishRelative[eatenList[i]].ChangeSushiType(SushiTypes.Empty);
        }
        
        ActivateResult();
    }
    
    #endregion

    /*
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

    private void CheckCondition(ConditionTypes condition, string valStr)
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
    */
}

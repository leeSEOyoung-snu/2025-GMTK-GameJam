using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionManager : MonoBehaviour, IInit
{
    public static InteractionManager Instance { get; private set; }

    public Dictionary<int, DishBehaviour> CatDishRelative;
    public Dictionary<int, DishBehaviour> passedDish;
    private Dictionary<int, bool> activationInfo;

    private Dictionary<int, bool> isSushiEaten;
    
    private int currCompletedRelative;

    private Queue<(ConditionTypes, string)> conditionQueue;
    private (ConditionTypes, string) currentCondition;

    public bool isProcessing;

    private int activatedId;
    
    private int currCompletedCondition;

    private bool isFirstTurn;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Init()
    {
        CatDishRelative = new Dictionary<int, DishBehaviour>();
        passedDish = new Dictionary<int, DishBehaviour>();
        activationInfo = new Dictionary<int, bool>();
        isSushiEaten = new Dictionary<int, bool>();
        currCompletedRelative = 0;

        isFirstTurn = true;
        
        conditionQueue = new Queue<(ConditionTypes, string)>();
        isProcessing = false;
        
        currCompletedCondition = 0;
    }

    #region Check Condition
    public void EnQueueCondition(ConditionTypes conditionType, string val1)
    {
        conditionQueue.Enqueue((conditionType, val1));
    }

    public void TriggerProcess(bool force = false)
    {
        if (!force && isProcessing) return;
        if (conditionQueue.Count == 0) return;
        isProcessing = true;
        activatedId = 0;
        currentCondition = conditionQueue.Dequeue();
        CheckCondition();
    }

    private void CheckCondition()
    {
        if (isFirstTurn)
        {
            InitCatDishRelative();
            isFirstTurn = false;
        }
        currCompletedCondition = 0;
        for (int i = 0; i < DiningManager.Instance.CatCnt; i++)
        {
            if (activationInfo[i]) continue;
            activationInfo[i] = DiningManager.Instance.CatBehaviourDict[i].CheckCondition(currentCondition.Item1, currentCondition.Item2);
        }

        if (conditionQueue.Count > 0)
        {
            currentCondition = conditionQueue.Dequeue();
            CheckCondition();
        }
        else
        {
            ActivateResult();
        }
    }

    public void ActivateResult()
    {
        bool noActive = true;
        foreach (bool active in activationInfo.Values)
        {
            if (active) noActive = false;
        }
        
        if (noActive)
        {
            isProcessing = false;
            MainSceneManager.Instance.CheckConditionCompleted();
            return;
        }

        activatedId++;
        if (!activationInfo[activatedId % DiningManager.Instance.CatCnt])
        {
            ActivateResult();
            return;
        }
        
        activationInfo[activatedId % DiningManager.Instance.CatCnt] = false;
        DiningManager.Instance.CatBehaviourDict[activatedId % DiningManager.Instance.CatCnt].ActivateResult();
    }
    
    #endregion
    
    #region Cat Dish Relative
    
    public void InitCatDishRelative()
    {
        if (CatDishRelative.Count == 0)
        {
            Debug.LogWarning("First");
            for (int i = 0; i < DiningManager.Instance.CatCnt; i++)
            {
                passedDish.Add(i, null);
                CatDishRelative.Add(i, null);
                activationInfo.Add(i, false);
                isSushiEaten.Add(i, false);
            }
        }
        
        for (int i = 0; i < DiningManager.Instance.CatCnt; i++)
        {
            passedDish[i] = CatDishRelative[i];
            CatDishRelative[i] = null;
            activationInfo[i] = false;
            isSushiEaten[i] = false;
        }

        foreach (var pair in passedDish)
        {
            Debug.Log($"passedDish: {pair.Key} - {pair.Value}");
        }
        
        activatedId = 0;
    }
    
    public void CheckCatDishRelative()
    {
        // TODO: passed 추가
        currCompletedRelative = 0;
        
        foreach (var pair in passedDish)
        {
            Debug.Log($"passedDish: {pair.Key} - {pair.Value}");
            if (pair.Value == null || activationInfo[pair.Key]) continue;
            CatBehaviour cat = DiningManager.Instance.CatBehaviourDict[pair.Key];
            if (cat.CheckCondition(ConditionTypes.SushiPassed, pair.Value.DishData.Sushi.ToString()))
                activationInfo[pair.Key] = true;
            if (cat.CheckCondition(ConditionTypes.DishPassed, pair.Value.DishData.Color.ToString()))
                activationInfo[pair.Key] = true;
                
        }
        
        foreach (var pair in CatDishRelative)
        {
            if (pair.Value == null)
            {
                // currCompletedRelative++;
                CheckRelativeCompleted();
                continue;
            }
            isSushiEaten[pair.Key] = DiningManager.Instance.CatBehaviourDict[pair.Key].TryEat(pair.Value.DishData.Color, pair.Value);
        }
    }
    
    public void CheckRelativeCompleted()
    {
        if (currCompletedRelative < DiningManager.Instance.CatCnt - 1) { currCompletedRelative++; return; }

        currCompletedRelative = 0;

        foreach (var pair in isSushiEaten)
        {
            if (!pair.Value) continue;
            if (!activationInfo[pair.Key])
            {
                activationInfo[pair.Key] = DiningManager.Instance.CatBehaviourDict[pair.Key]
                    .CheckCondition(ConditionTypes.SushiEaten, CatDishRelative[pair.Key].DishData.Sushi.ToString());
            }
            CatDishRelative[pair.Key].ChangeSushiType(SushiTypes.Empty);
        }
        
        foreach (var pair in activationInfo)
            Debug.Log($"activationInfo: {pair.Key} - {pair.Value}");
        
        ActivateResult();
    }
    
    #endregion
}

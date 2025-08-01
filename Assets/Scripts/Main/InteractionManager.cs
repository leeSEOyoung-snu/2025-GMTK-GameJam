using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour, IInit
{
    public static InteractionManager Instance { get; private set; }

    public Dictionary<int, DishBehaviour> CatDishRelative;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Init()
    {
        
    }

    public void InitRelative()
    {
        for (int i = 0; i < DiningManager.Instance.CatCnt; i++)
        {
            CatDishRelative.Add(i, null);
        }
    }
}

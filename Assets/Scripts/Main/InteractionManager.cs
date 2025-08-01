using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour, IInit
{
    public static InteractionManager Instance { get; private set; }
    

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Init()
    {
        
    }
}

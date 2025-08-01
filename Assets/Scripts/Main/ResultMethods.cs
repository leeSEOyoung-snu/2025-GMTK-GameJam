using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResultTypes
{
    GenerateCard,
    
}

public class ResultMethods : MonoBehaviour
{
    public static ResultMethods Instance { get; private set; }
    
    [SerializeField] private List<Sprite> iconSprites;
    [SerializeField] private List<Sprite> sushiSprites;
    [SerializeField] private List<Sprite> dishSprites;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitVisitor : MonoBehaviour
{
    private List<IInit> _initScripts = new List<IInit>();
    
    private void Awake()
    {
        _initScripts = new List<IInit>(transform.GetComponentsInChildren<IInit>());
    }

    private void Start()
    {
        // foreach(IInit script in _initScripts) script.Init();
    }
}

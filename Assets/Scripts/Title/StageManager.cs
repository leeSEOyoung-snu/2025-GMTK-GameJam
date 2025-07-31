using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region Fields
    private GameObject StagePanel;
    
    #endregion


    #region LifeCycle
    private void Awake()
    {
        // Find the StagePanel GameObject in the scene
        StagePanel = GameObject.Find("StagePanel");
        if (StagePanel == null) Debug.LogError("StagePanel not found in the scene.");
        
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}

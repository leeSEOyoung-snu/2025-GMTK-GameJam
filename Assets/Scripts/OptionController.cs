using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    public void BackButtonClicked()
    {
        Debug.Log("Back button clicked");
        // Example: Close the options panel
        gameObject.SetActive(false);
    }
}

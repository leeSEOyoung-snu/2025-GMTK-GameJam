using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    #region Fields
    private int currentSelcectedButton = -1;
    private int previousSelectedButton = -1;
    private List<TextMeshProUGUI> Buttons = new List<TextMeshProUGUI>();
    private GameObject TitlePanel;
    #endregion
    
    #region LifeCycle
    private void Awake()
    {
        // Find the TitlePanel GameObject in the scene
        TitlePanel = GameObject.Find("TitlePanel");
        if (TitlePanel == null) Debug.LogError("TitlePanel not found in the scene.");
        
        // Find all buttons in the scene and add them to the list
        foreach(TextMeshProUGUI button in TitlePanel.GetComponentsInChildren<TextMeshProUGUI>()) Buttons.Add(button);
        Debug.Log(Buttons.Count + " buttons found in the scene.");
    }
    void Update()
    {
        //currentSelectedButton Logic Part
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            previousSelectedButton = currentSelcectedButton;
            currentSelcectedButton--;
            if(currentSelcectedButton < -1 || previousSelectedButton == 0) { currentSelcectedButton = Buttons.Count - 1; }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            previousSelectedButton = currentSelcectedButton; 
            currentSelcectedButton++;
            if(currentSelcectedButton > Buttons.Count - 1){ currentSelcectedButton = 0;}
        }
        
        Debug.Log(currentSelcectedButton + " is the current selected button index");
        
        //Change detected
        if (previousSelectedButton != currentSelcectedButton)
        {
            ButtonHighLighting();
        }

        //push the button by enter key
        if (Input.GetKeyDown(KeyCode.Return) && currentSelcectedButton != -1)
        {
            Debug.Log("Button " + currentSelcectedButton + " clicked");
            switch (currentSelcectedButton)
            {
                case 0:
                    OnClickStartButton();
                    break;
                case 1:
                    OnClickOptionButton();
                    break;
                case 2:
                    OnClickQuitButton();
                    break;
                default:
                    Debug.LogWarning("No button assigned for index: " + currentSelcectedButton);
                    break;
            }
        }
    }
    #endregion

    private void ButtonHighLighting()
    {
        if (currentSelcectedButton >= 0 && currentSelcectedButton <= Buttons.Count - 1)
        {
            Buttons[currentSelcectedButton].color = Color.white;
            if(previousSelectedButton != -1) Buttons[previousSelectedButton].color = Color.black;
        }
    }

    #region ButtonMethods
    public void OnClickStartButton()
    {
        Debug.Log("Start button clicked");
        TitlePanel.SetActive(false);
        
    }

    public void OnClickOptionButton()
    {
        Debug.Log("Option button clicked");
    }

    public void OnClickQuitButton()
    {
        // Quit the application
        Debug.Log("Quit button clicked");
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }

    public void OnClickTitleButton()
    {
        initTitleManager();
        TitlePanel.SetActive(true);
    }

    #endregion

    private void initTitleManager()
    {
        currentSelcectedButton = -1;
        previousSelectedButton = -1;
        foreach (TextMeshProUGUI button in Buttons) button.color = Color.black; // Reset all buttons to black
        
    }
}

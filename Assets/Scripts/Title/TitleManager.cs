using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    #region Fields
    private int currentSelcectedButton = -1;
    private int previousSelectedButton = -1;
    private List<GameObject> Buttons = new List<GameObject>();
    [Header("Buttons")]
    [SerializeField] private GameObject StartButton;
    [SerializeField] private GameObject OptionButton;
    [SerializeField] private GameObject QuitButton;

    [Header("Panels")] 
    [SerializeField] private GameObject TitlePanel;
    [SerializeField] private GameObject OptionPanel;
    #endregion
    
    #region LifeCycle
    private void Awake()
    {
        // Find the TitlePanel GameObject in the scene
        if (TitlePanel == null) Debug.LogWarning("TitlePanel not found in the scene.");
        if (OptionPanel == null) Debug.LogWarning("OptionPanel not found in the scene.");
        
        TitlePanel.SetActive(true);
        OptionPanel.SetActive(false);
        
        // Find all buttons in the scene and add them to the list
        Buttons.Add(StartButton);
        Buttons.Add(OptionButton);
        Buttons.Add(QuitButton);
        
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
        
        //Debug.Log(currentSelcectedButton + " is the current selected button index");
        
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
            //TODO : Change TMPpro To Image
            Buttons[currentSelcectedButton].GetComponent<Image>().color = new Color(1,1,1,0.5f);
            if(previousSelectedButton != -1) Buttons[previousSelectedButton].GetComponent<Image>().color = Color.white;
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
        OptionPanel.SetActive(true);
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
        foreach (GameObject button in Buttons) button.GetComponent<Image>().color = Color.white; // Reset all buttons to black
        
    }
}

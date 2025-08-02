using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    [SerializeField] Scrollbar BGMVolumeScrollbar;
    [SerializeField] Scrollbar SFXVolumeScrollbar;

    [SerializeField] private GameObject BackTotheTitleButton;
    [SerializeField] private GameObject BackTotheGameButton;
    private string SceneName;

    private void Awake()
    {
        BGMVolumeScrollbar.value = 0.5f;
        SFXVolumeScrollbar.value = 0.5f;
    }

    private void Start()
    {
        //BacktotheGameButton
        SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (SceneName == "Title") Destroy(BackTotheTitleButton);


        //SoundManager
        SoundManager.Instance.backgroundMusicSource.volume = BGMVolumeScrollbar.value;
        SoundManager.Instance.soundEffectSource.volume = SFXVolumeScrollbar.value;
        BGMVolumeScrollbar.onValueChanged.AddListener((value) =>
        {
            SoundManager.Instance.backgroundMusicSource.volume = value;
        });

        SFXVolumeScrollbar.onValueChanged.AddListener((value) =>
        {
            SoundManager.Instance.soundEffectSource.volume = value;
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed");
            if (SceneName == "Title")
            {
                BackButtonClicked();
            }
            else
            {
                // During Gaming Close the options panel
                gameObject.SetActive(false);
            }
        }
    }

    public void ScreenModeChanged(int value)
    {
        switch (value)
        {
            case 0: // Fullscreen
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Debug.Log("Fullscreen mode selected");
                break;
            case 1: // Windowed
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                Debug.Log("Windowed mode selected");
                break;
            default:
                Debug.LogWarning("Unknown screen mode selected.");
                Debug.Log("Fucked up!");
                break;
        }
    }

    public void TitleButonClicked()
    {
        if (SceneName != "Title")
        {
            SceneManager.LoadScene("Title");
            return;
        }
    }

    public void BackButtonClicked()
    {
        Debug.Log("Back button clicked");
        // Example: Close the options panel
        gameObject.SetActive(false);
    }

    public void OnHoverEnter(BaseEventData data)
    {
        PointerEventData ped = (PointerEventData)data;
        GameObject hoveredObject = ped.pointerEnter;

        if (hoveredObject == BackTotheGameButton)
        {
            BackTotheGameButton.GetComponent<RectTransform>().localScale =
                new Vector3(1.2f, 1.2f, 1f);
        }
        else if (hoveredObject == BackTotheTitleButton)
        {
            BackTotheTitleButton.GetComponent<RectTransform>().localScale =
                new Vector3(1.2f, 1.2f, 1f);
        }
    }

    public void OnHoverExit(BaseEventData data)
    {
        initButtonSize();
    }

    private void initButtonSize()
    {
        BackTotheGameButton.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1f);
        if(SceneName != "Title") BackTotheTitleButton.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1f);
    }
}



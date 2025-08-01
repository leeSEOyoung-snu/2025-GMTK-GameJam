using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    [SerializeField] Scrollbar BGMVolumeScrollbar;
    [SerializeField] Scrollbar SFXVolumeScrollbar;

    private void Awake()
    {
        BGMVolumeScrollbar.value = 0.5f;
        SFXVolumeScrollbar.value = 0.5f;
    }

    private void Start()
    {
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

    public void BackButtonClicked()
    {
        Debug.Log("Back button clicked");
        // Example: Close the options panel
        gameObject.SetActive(false);
    }
}

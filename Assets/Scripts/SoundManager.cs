using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] public AudioSource backgroundMusicSource;
    [SerializeField] public AudioSource soundEffectSource;
    [SerializeField] public AudioClip[] BGMs;
    [SerializeField] public AudioClip[] SFXs;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        if (backgroundMusicSource == null || soundEffectSource == null)
        {
            Debug.LogError("AudioSources are not assigned in SoundManager.");
            return;
        }
        if(SceneManager.GetActiveScene().name == "Title")
        {
            PlayBGM(BGMs[0]); // Play the first BGM on MainScene
        }
        else
        {
            PlayBGM(BGMs[1]); // Play the second BGM on other scenes
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        backgroundMusicSource.clip = clip;
        backgroundMusicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        soundEffectSource.PlayOneShot(clip);
    }
}

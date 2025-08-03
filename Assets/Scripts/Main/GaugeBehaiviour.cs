using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GaugeBehaiviour : MonoBehaviour
{
    [SerializeField] private Image gaugeBar;
    [SerializeField] private Image FireAnimationImage;
    [SerializeField] private TextMeshProUGUI TargetScore;
    [SerializeField] private TextMeshProUGUI CurrentScore;
    [SerializeField] private Sprite FireAnimiationSprite1;
    [SerializeField] private Sprite FireAnimiationSprite2;
    private float gaugeValue = 0f; // Current gauge value

    private bool isFire = false;
    [SerializeField] float fireAnimationTime; // Time for the fire animation to complete
    private float AnimationTime;

    void Start()
    {
        //for test
        UpdateText(MainSceneManager.Instance._currScore, MainSceneManager.Instance._targetScore);
        UpdateGaugeFinite(MainSceneManager.Instance._currScore, MainSceneManager.Instance._targetScore);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFire == true)
        {
            if (AnimationTime >= fireAnimationTime)
            {
                AnimationTime = 0f;
                if(FireAnimationImage.sprite == FireAnimiationSprite2)
                {
                    FireAnimationImage.sprite = FireAnimiationSprite1;
                }
                else
                {
                    FireAnimationImage.sprite = FireAnimiationSprite2;
                }
            }
            AnimationTime += Time.deltaTime;
        }
    }

    
    
    public void UpdateGaugeFinite(int currentValue, int maxValue)
    {
        
        gaugeValue = Mathf.Clamp01((float)currentValue / maxValue);
        gaugeBar.fillAmount = gaugeValue;
    }

    public void UpdateGaugeInfinite()
    {
        // For infinite gauge, we can just set it to full
        gaugeBar.fillAmount = 1;
        this.isFire = true;
    }
    
    public void UpdateText(int currentScore, int targetScore) {
        CurrentScore.text = currentScore.ToString();
        TargetScore.text = targetScore.ToString();
    }
    

}

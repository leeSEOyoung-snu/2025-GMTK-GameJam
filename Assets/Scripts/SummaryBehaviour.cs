using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SummaryBehaviour : MonoBehaviour
{
    [SerializeField] private Sprite NextStageButtonSprite;
    [SerializeField] private Sprite RetryButtonSprite;
    [SerializeField] private TextMeshProUGUI MessageText;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private Image VariButton;
    private int currScore, targetScore;
    
    private bool isClear; // true if the stage is cleared, false otherwise
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetSummary(bool isClear, int currScore, int targetScore)
    {
        this.isClear = isClear;
        this.currScore = currScore;
        this.targetScore = targetScore;
        if (isClear)
        {
            MessageText.text = "Stage Cleared!";
            ScoreText.text = "Score: " + currScore + " / " + targetScore;
            VariButton.sprite = NextStageButtonSprite;
        }
        else
        {
            MessageText.text = "Stage Failed!";
            ScoreText.text = "Score: " + currScore + " / " + targetScore;
            VariButton.sprite = RetryButtonSprite;
        }
    }

    public void OnTitleButtonClicked()
    {
        SceneManager.LoadScene("Scenes/Title");
    }
    
    public void OnNextOrReButtonClicked()
    {
        if (isClear)    //nextButton
        {
            GameManager.Instance.EndStage();
        }
        else
        {
            // Retry the current stage
            MainSceneManager.Instance.Init();
        }
    }


}

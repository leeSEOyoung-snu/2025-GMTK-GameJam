using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SummaryBehaviour : MonoBehaviour
{
    [SerializeField] private Sprite NextStageButtonSprite;
    [SerializeField] private Sprite RetryButtonSprite;
    [SerializeField] private Image Image;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private Image TitleButton;
    [SerializeField] private Image VariButton;
    [SerializeField] private Sprite WinCatSprite;
    [SerializeField] private Sprite LoseCatSprite;
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
        ScoreText.text = currScore.ToString() + " / " + targetScore.ToString();
        if (isClear)
        {
            Image.sprite = WinCatSprite;
            VariButton.sprite = NextStageButtonSprite;
        }
        else
        {
            Image.sprite = LoseCatSprite;
            VariButton.sprite = RetryButtonSprite;
        }
    }

    public void OnTitleButtonClicked()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.SFXs[0]);
        SceneManager.LoadScene("Scenes/Title");
    }
    
    public void OnNextOrReButtonClicked()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.SFXs[0]);
        if (isClear)    //nextButton
        {
            this.gameObject.SetActive(false);
            GameManager.Instance.EndStage();
        }
        else //retryButton
        {
            this.gameObject.SetActive(false);
            // Retry the current stage
            MainSceneManager.Instance.Init();
        }
    }
    
    public void OnHoverEnter(BaseEventData data)
    {
        PointerEventData ped = (PointerEventData)data;
        GameObject hoveredObject = ped.pointerEnter;

        hoveredObject.GetComponent<RectTransform>().localScale =
            new Vector3(1.2f, 1.2f, 1f); // Scale up the hovered button
        
    }

    public void OnHoverExit(BaseEventData data)
    {
        initButtonSize();
    }

    private void initButtonSize()
    {
        TitleButton.GetComponent<RectTransform>().localScale =
            new Vector3(1f, 1f, 1f); // Reset scale of the button
        VariButton.GetComponent<RectTransform>().localScale =
            new Vector3(1f, 1f, 1f); // Reset scale of the button
    }

}

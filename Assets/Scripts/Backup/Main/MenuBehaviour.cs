using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuBehaviour : MonoBehaviour
{
    [SerializeField] private Image sushiImg;
    [SerializeField] private RectTransform sushiRect;
    [SerializeField] private TextMeshProUGUI priceText;

    public void InitMenu(SushiTypes sushi)
    {
        sushiImg.sprite = TableManager.Instance.sushiSprites[(int)sushi];
        sushiRect.sizeDelta = TableManager.Instance.sushiSprites[(int)sushi].rect.size;
        priceText.text = $"{MainSceneManager.Instance.Price[sushi]}\u00a5";
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour, IInit
{
    public static CardManager Instance { get; private set; }

    [SerializeField] private GameObject cardPref, cardParent;
    
    public List<Sprite> cardSprites;
    
    public List<CardBehaviour> CurrCards { get; private set; }
    public readonly float CardMoveDuration = 0.5f;

    private readonly float _cardMinPosX = -4f, _cardMaxPosX = 3f, _cardNormalPosY = 0f;
    public readonly float CardHoveredPosY = 0.7f;

    private readonly int _defaultOrder = 10;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Init()
    {
        string[] cardData = CSVReader.ParseDollar((string)MainSceneManager.Instance.CurrStageData["StartSushi"]);
        if (cardData.Length < 1) { Debug.LogError($"something's wrong [CurrCardCnt == {cardData.Length}]"); return; }
        
        foreach (Transform card in cardParent.transform)
            Destroy(card.gameObject);
        
        CurrCards = new List<CardBehaviour>();
        
        float cardGap = (_cardMaxPosX - _cardMinPosX) / (cardData.Length - 1);

        for (int i = 0; i < cardData.Length; i++)
        {
            var cardBehaviour = Instantiate(cardPref, cardParent.transform).GetComponent<CardBehaviour>();
            if (Enum.TryParse(cardData[i], ignoreCase: true, out SushiTypes sushi))
            {
                cardBehaviour.InitCard(sushi, new Vector3(_cardMinPosX + cardGap * i, _cardNormalPosY, 0f), _defaultOrder + i);
                CurrCards.Add(cardBehaviour);
            }
            else Debug.LogError("Card Sushi Error: " + cardData[i]);
        }
    }

    private void GenerateCard(SushiTypes sushi)
    {
        
    }
}

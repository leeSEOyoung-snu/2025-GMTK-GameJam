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

    private CardBehaviour _selectedCard;

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

        for (int i = 0; i < cardData.Length; i++)
        {
            var cardBehaviour = Instantiate(cardPref, cardParent.transform).GetComponent<CardBehaviour>();
            if (Enum.TryParse(cardData[i], ignoreCase: true, out SushiTypes sushi))
            {
                cardBehaviour.InitCard(sushi, Vector3.zero, _defaultOrder + i);
                CurrCards.Add(cardBehaviour);
            }
            else Debug.LogError("Card Sushi Error: " + cardData[i]);
        }
        
        ArrangeCard();
    }

    private void ArrangeCard()
    {
        float cardGap = (_cardMaxPosX - _cardMinPosX) / (CurrCards.Count - 1);
        if (CurrCards.Count == 1)
        {
            CurrCards[0].ChangePosition(new Vector3(0f, _cardNormalPosY, 0f), _defaultOrder);
            return;
        }
        for (int i = 0; i < CurrCards.Count; i++)
        {
            CurrCards[i].ChangePosition(new Vector3(_cardMinPosX + cardGap * i, _cardNormalPosY, 0f), _defaultOrder + i);
        }
    }

    private void DiscardCard(CardBehaviour card)
    {
        CurrCards.Remove(card);
        Destroy(card.gameObject);
        ArrangeCard();
    }

    public void CardSelected(CardBehaviour cardBehaviour)
    {
        if (_selectedCard != null)
        {
            _selectedCard.OnPointerClick(null);
        }
        
        _selectedCard = cardBehaviour;
    }

    public void CardDeselected()
    {
        _selectedCard = null;
    }

    public void ConditionSelected(CatConditionBehaviour conditionBehaviour)
    {
        if (_selectedCard == null) return;
        conditionBehaviour.SetCondition(_selectedCard.Sushi);
        DiscardCard(_selectedCard);
        _selectedCard = null;
    }

    public void PutSushiOnDish(DishBehaviour dish)
    {
        if (_selectedCard == null) return;
        dish.PutSushiOnDish(_selectedCard.Sushi);
        DiscardCard(_selectedCard);
        _selectedCard = null;
    }
}

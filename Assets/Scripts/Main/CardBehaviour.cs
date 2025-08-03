using System.Diagnostics.Tracing;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public SushiTypes Sushi { get; private set; }

    [SerializeField] private SpriteRenderer cardSr;

    private Vector3 _normalPos, _hoveredPos;
    private Sequence _sequence;
    
    private readonly int _hoveredOrderAdder = 15;
    private int _order;
    
    private bool isSelected;

    public void InitCard(SushiTypes sushi, Vector3 normalPos, int order)
    {
        Sushi = sushi;
        
        _normalPos = normalPos;
        transform.localPosition = normalPos;
        _hoveredPos = new Vector3(normalPos.x, normalPos.y + CardManager.Instance.CardHoveredPosY, normalPos.z);
        
        _order = order;
        cardSr.sortingOrder = order;
        cardSr.sprite = CardManager.Instance.cardSprites[(int)sushi];
        
        isSelected = false;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CardManager.Instance.isCardChanging || isSelected) return;
        cardSr.sortingOrder = _order + _hoveredOrderAdder;
        if (_sequence != null && _sequence.IsActive() && _sequence.IsPlaying())
        {
            _sequence.Kill();
        }
        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOLocalMove(_hoveredPos, CardManager.Instance.CardMoveDuration));
        _sequence.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected) return;
        cardSr.sortingOrder = _order;
        if (_sequence != null && _sequence.IsActive() && _sequence.IsPlaying())
        {
            _sequence.Kill();
        }
        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOLocalMove(_normalPos, CardManager.Instance.CardMoveDuration));
        _sequence.Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CardManager.Instance.isCardChanging) return;
        if (isSelected)
        {
            isSelected = false;
            CardManager.Instance.CardDeselected();
            OnPointerExit(null);
        }
        else
        {
            isSelected = true;
            CardManager.Instance.CardSelected(this);
            OnPointerEnter(null);
        }
    }

    public void ChangePosition(Vector3 normalPos, int newOrder)
    {
        _order = newOrder;
        cardSr.sortingOrder = newOrder;
        _normalPos = normalPos;
        _hoveredPos = new Vector3(normalPos.x, normalPos.y + CardManager.Instance.CardHoveredPosY, normalPos.z);
        if (_sequence != null && _sequence.IsActive() && _sequence.IsPlaying())
        {
            _sequence.Kill();
        }
        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOLocalMove(_normalPos, CardManager.Instance.CardMoveDuration));
        _sequence.Play();
    }

    public void ChangeSushiUp(SushiTypes sushi)
    {
        if (_sequence != null && _sequence.IsActive() && _sequence.IsPlaying())
        {
            _sequence.Kill();
        }
        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOLocalMove(_hoveredPos, CardManager.Instance.CardMoveDuration));
        _sequence.Play().OnComplete(() => { ChangeSushiDown(sushi); });
    }

    public void ChangeSushiDown(SushiTypes sushi)
    {
        Sushi = sushi;
        cardSr.sprite = CardManager.Instance.cardSprites[(int)sushi];
        if (_sequence != null && _sequence.IsActive() && _sequence.IsPlaying())
        {
            _sequence.Kill();
        }
        _sequence = DOTween.Sequence();
        _sequence.Insert(1f, transform.DOLocalMove(_normalPos, CardManager.Instance.CardMoveDuration));
        _sequence.Play().OnComplete(CardManager.Instance.EndChangeCard);
    }
}

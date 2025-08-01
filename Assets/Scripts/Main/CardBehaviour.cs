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

    private int order;
    private readonly float _hoveredPosZ = 1f;

    public void InitCard(SushiTypes sushi, Vector3 normalPos, int order)
    {
        Sushi = sushi;
        
        _normalPos = normalPos;
        transform.localPosition = normalPos;
        _hoveredPos = new Vector3(normalPos.x, normalPos.y + CardManager.Instance.CardHoveredPosY, normalPos.z);

        this.order = order;
        cardSr.sortingOrder = order;
        cardSr.sprite = CardManager.Instance.cardSprites[(int)sushi];
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter Card");
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, _hoveredPosZ);
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
        Debug.Log("Exit Card");
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);
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
        throw new System.NotImplementedException();
    }

    public void ChangePosition(Vector3 normalPos, int newOrder)
    {
        order = newOrder;
        cardSr.sortingOrder = newOrder;
        if (_sequence != null && _sequence.IsActive() && _sequence.IsPlaying())
        {
            _sequence.Kill();
        }
        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOLocalMove(_normalPos, CardManager.Instance.CardMoveDuration));
        _sequence.Play();
        transform.position = normalPos;
        _hoveredPos = new Vector3(normalPos.x, normalPos.y + CardManager.Instance.CardHoveredPosY, normalPos.z);
    }
}

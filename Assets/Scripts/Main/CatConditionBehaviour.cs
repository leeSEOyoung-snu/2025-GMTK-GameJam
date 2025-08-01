using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CatConditionBehaviour : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CatBehaviour catBehaviour;
    [SerializeField] private SpriteRenderer iconSr;
    [SerializeField] private GameObject typePref;
    [SerializeField] private Collider2D conditionCollider;
    
    private GameObject type;

    private bool isStandBy;

    private readonly float _typePosX = 0.5f;
    private readonly float _sushiScale = 0.6f, _dishScale = 0.4f, _highlightFactor = 1.2f;

    public void InitCondition(Sprite[] sprites, bool isStandBy, bool isSushiType)
    {
        iconSr.sprite = sprites[0];
        
        type = Instantiate(typePref, transform);
        type.transform.localPosition = new Vector3(_typePosX, 0, 0);
        if (isSushiType) type.transform.localScale = new Vector3(_sushiScale, _sushiScale, 0);
        else type.transform.localScale = new Vector3(_dishScale, _dishScale, 0);
        type.GetComponent<SpriteRenderer>().sprite = sprites[1];
        
        this.isStandBy = isStandBy;
        conditionCollider.enabled = isSushiType;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}

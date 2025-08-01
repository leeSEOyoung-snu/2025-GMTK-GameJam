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
    
    private Transform typeTransform;

    private readonly float _typePosX = 0.5f;
    private readonly float _sushiScale = 0.6f, _dishScale = 0.4f, _highlightFactor = 1.2f;

    public void InitCondition(Sprite[] sprites, bool isStandBy, bool isSushiType)
    {
        iconSr.sprite = sprites[0];
        
        typeTransform = Instantiate(typePref, transform).GetComponent<Transform>();
        typeTransform.localPosition = new Vector3(_typePosX, 0, 0);
        if (isSushiType) typeTransform.localScale = new Vector3(_sushiScale, _sushiScale, 0);
        else typeTransform.localScale = new Vector3(_dishScale, _dishScale, 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}

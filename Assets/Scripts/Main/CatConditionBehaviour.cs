using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CatConditionBehaviour : IPointerHandler
{
    [SerializeField] private CatBehaviour catBehaviour;
    [SerializeField] private SpriteRenderer iconSr;
    [SerializeField] private GameObject typePref;
    [SerializeField] private GameObject pointerDetector;
    
    private GameObject type;

    private bool isStandBy;

    private readonly float _typePosX = 0.5f;
    private readonly float _sushiScale = 0.6f, _dishScale = 0.4f, _highlightFactor = 1.2f;

    public void InitCondition(Sprite[] sprites, bool isStandBy, bool isSushiType)
    {
        Debug.Log($"stand: {isStandBy}, sushi: {isSushiType}");
        iconSr.sprite = sprites[0];
        
        type = Instantiate(typePref, transform);
        type.transform.SetAsFirstSibling();
        type.transform.localPosition = new Vector3(_typePosX, 0, 0);
        if (isSushiType) type.transform.localScale = new Vector3(_sushiScale, _sushiScale, 0);
        else type.transform.localScale = new Vector3(_dishScale, _dishScale, 0);
        type.GetComponent<SpriteRenderer>().sprite = sprites[1];
        
        this.isStandBy = isStandBy;
        pointerDetector.SetActive(isStandBy);
    }

    public override void HandlePointerClick()
    {
        if (MainSceneManager.Instance.CookStarted) return;
        Debug.Log("Card Condition Clicked");
        CardManager.Instance.ConditionSelected(this);
    }
    
    public override void HandlePointerEnter() {}
    
    public override void HandlePointerExit() {}

    public void SetCondition(SushiTypes condition)
    {
        isStandBy = false;
        pointerDetector.SetActive(false);
        type.GetComponent<SpriteRenderer>().sprite = TableManager.Instance.sushiSprites[(int)condition];
        catBehaviour.Condition = condition.ToString();
    }
}

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

    public void InitCondition(Sprite[] sprites, bool isStandBy, bool isSushiType)
    {
        iconSr.sprite = sprites[0];
        
        type = Instantiate(typePref, transform);
        type.transform.SetAsFirstSibling();
        type.transform.localPosition = new Vector3(DiningManager.Instance.BubbleTypePosX, 0, 0);
        if (isSushiType) type.transform.localScale = new Vector3(DiningManager.Instance.BubbleSushiScale, DiningManager.Instance.BubbleSushiScale, 0);
        else type.transform.localScale = new Vector3(DiningManager.Instance.BubbleDishScale, DiningManager.Instance.BubbleDishScale, 0);
        type.GetComponent<SpriteRenderer>().sprite = sprites[1];
        
        pointerDetector.SetActive(isStandBy);
    }

    public override void HandlePointerClick()
    {
        Debug.Log("HandlePointerClick");
        if (MainSceneManager.Instance.CookStarted) return;
        CardManager.Instance.ConditionSelected(this);
    }
    
    public override void HandlePointerEnter() {}
    
    public override void HandlePointerExit() {}

    public void SetCondition(SushiTypes condition)
    {
        pointerDetector.SetActive(false);
        type.GetComponent<SpriteRenderer>().sprite = TableManager.Instance.sushiSprites[(int)condition];
        catBehaviour.Condition = condition.ToString();
    }
}

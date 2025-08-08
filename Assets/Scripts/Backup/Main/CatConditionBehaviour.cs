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

    public void InitCondition(Sprite[] sprites, bool isStandBy, bool isSushiType)
    {
        iconSr.sprite = sprites[0];
        
        this.isStandBy = isStandBy;
        
        type = Instantiate(typePref, transform);
        type.transform.SetAsFirstSibling();
        type.transform.localPosition = new Vector3(DiningManager.Instance.BubbleTypePosX, 0, 0);
        if (isSushiType)
        {
            if (isStandBy) type.transform.localScale = new Vector3(1, 1, 1);
            else
            {
                type.transform.localScale = new Vector3(DiningManager.Instance.BubbleSushiScale,
                    DiningManager.Instance.BubbleSushiScale, 0);
            }
        }
        else if (ConditionMethods.Instance.dishSprites.IndexOf(sprites[0]) != 5)
            type.transform.localScale = new Vector3(1, 1, 0);
        else type.transform.localScale = new Vector3(DiningManager.Instance.BubbleDishScale, DiningManager.Instance.BubbleDishScale, 0);
        type.GetComponent<SpriteRenderer>().sprite = sprites[1];
        
        pointerDetector.SetActive(true);
    }

    public override void HandlePointerClick()
    {
        if (MainSceneManager.Instance.CookStarted || !isStandBy) return;
        SoundManager.Instance.PlaySFX(SoundManager.Instance.SFXs[5]);
        CardManager.Instance.ConditionSelected(this);
    }

    public override void HandlePointerEnter()
    {
        TooltipManager.Instance.ShowTooltip();
        TooltipManager.Instance.setupTooltip(catBehaviour.conditionType, Camera.main.WorldToScreenPoint(this.transform.position)+new Vector3(0,+75f,0));

    }

    public override void HandlePointerExit()
    {
        TooltipManager.Instance.HideTooltip();
    }

    public void SetCondition(SushiTypes condition)
    {
        isStandBy = false;
        type.GetComponent<SpriteRenderer>().sprite = TableManager.Instance.sushiSprites[(int)condition];
        type.transform.localScale = new Vector3(DiningManager.Instance.BubbleSushiScale,
            DiningManager.Instance.BubbleSushiScale, 0);
        catBehaviour.Condition = condition.ToString();
    }
}

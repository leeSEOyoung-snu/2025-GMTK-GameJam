using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerDetector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private IPointerHandler _clickHandler;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        _clickHandler.HandlePointerClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _clickHandler.HandlePointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _clickHandler.HandlePointerExit();
    }
}

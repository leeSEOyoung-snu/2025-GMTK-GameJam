using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotateButtonBehaviour : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject rotateCnt;
    [SerializeField] private Image sr;
    [SerializeField] private Sprite normal, hovered;

    public void OnPointerClick(PointerEventData eventData)
    {
        MainSceneManager.Instance.Rotate();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        sr.sprite = hovered;
        rotateCnt.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        sr.sprite = normal;
        rotateCnt.SetActive(true);
    }
}

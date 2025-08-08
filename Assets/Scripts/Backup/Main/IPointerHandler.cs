using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class IPointerHandler : MonoBehaviour
{
    public abstract void HandlePointerClick();
    public abstract void HandlePointerEnter();
    public abstract void HandlePointerExit();
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupBehaviour : MonoBehaviour
{
    public List<Sprite> PopupSpritesList;
    
    [SerializeField] private Image popupImage;
    [SerializeField] private TextMeshProUGUI popupDescription;
    [SerializeField] private Image checkButton;
    // Start is called before the first frame update

    public void InitPopup(int popupIndex)
    {
        if(popupIndex < PopupSpritesList.Count-1)
        {
            Debug.Log("PopupSpritesList is null or index out of range");
            return;
        }
        popupImage.sprite = PopupSpritesList[popupIndex];
        popupImage.SetNativeSize(); // Adjust the size of the image to fit the sprite
        //for test
    }

    public void CheckButtonClicked()
    {
        Destroy(this.gameObject);
    }
    
    public void OnHoverEnter(BaseEventData data)
    {
        PointerEventData ped = (PointerEventData)data;
        GameObject hoveredObject = ped.pointerEnter;

        hoveredObject.GetComponent<RectTransform>().localScale =
            new Vector3(1.2f, 1.2f, 1f); // Scale up the hovered button
    }
    
    public void OnHoverExit(BaseEventData data)
    {
        initButtonSize();
    }

    private void initButtonSize()
    {
        checkButton.GetComponent<RectTransform>().localScale =
            new Vector3(1f, 1f, 1f); // Reset scale of the button
    }
}

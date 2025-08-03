using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextSushiBehaiviour : MonoBehaviour
{
    [SerializeField] private GameObject CardImagePrefab;

    public void Setup(int Sushicnt, List<string> _nextSushi)
    {
        switch (Sushicnt)
        {
            case 0:
                break;
            case 1:
                GameObject go = Instantiate(CardImagePrefab);
                go.transform.SetParent(transform, false);
                //Fucking Shit Hard coding And I really hate this.
                go.GetComponent<RectTransform>().localPosition = new Vector3(215, -190, 0);
                Enum.TryParse<SushiTypes>(_nextSushi[0], out SushiTypes sushiType);
                go.GetComponent<Image>().sprite = CardManager.Instance.cardSprites[(int)sushiType];
                break;
            case 2:
                GameObject go1 = Instantiate(CardImagePrefab);
                GameObject go2 = Instantiate(CardImagePrefab);
                go1.transform.SetParent(transform, false);
                go2.transform.SetParent(transform, false);
                //Fucking Shit Hard coding And I really hate this.
                go1.GetComponent<RectTransform>().position = new Vector3(215, -190, 0) + new Vector3(-80, -55, 0);
                go2.GetComponent<RectTransform>().position = new Vector3(215, -190, 0) + new Vector3(80, -55, 0);
                Enum.TryParse<SushiTypes>(_nextSushi[0], out SushiTypes sushiType1);
                Enum.TryParse<SushiTypes>(_nextSushi[1], out SushiTypes sushiType2);
                go1.GetComponent<Image>().sprite = CardManager.Instance.cardSprites[(int)sushiType1];
                go2.GetComponent<Image>().sprite = CardManager.Instance.cardSprites[(int)sushiType2];
                break;
            default:
                Debug.LogError("There is fucking somthing wrong with csv of nextSushi!!!");
                break;
        }
    }
}

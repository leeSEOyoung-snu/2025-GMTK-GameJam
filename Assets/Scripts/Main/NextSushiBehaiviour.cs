using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextSushiBehaiviour : MonoBehaviour
{
    [SerializeField] private GameObject CardImagePrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(int Sushicnt, List<string> _nextSushi)
    {
        switch (Sushicnt)
        {
            case 0:
                break;
            case 1:
                GameObject go = Instantiate(CardImagePrefab, this.transform);
                go.transform.SetParent(transform, false);
                go.transform.position += new Vector3(0, -30, 0);
                Enum.TryParse<SushiTypes>(_nextSushi[0], out SushiTypes sushiType);
                go.GetComponent<Image>().sprite = CardManager.Instance.cardSprites[(int)sushiType];
                break;
            case 2:
                GameObject go1 = Instantiate(CardImagePrefab, transform);
                GameObject go2 = Instantiate(CardImagePrefab, transform);
                go1.transform.SetParent(transform, false);
                go2.transform.SetParent(transform, false);
                go1.transform.position += new Vector3(-80, -30, 0);
                go2.transform.position += new Vector3(80, -30, 0);
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

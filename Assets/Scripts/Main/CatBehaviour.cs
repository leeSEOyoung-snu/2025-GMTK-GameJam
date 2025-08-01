using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat
{
    
}

public class CatBehaviour : MonoBehaviour
{
    public Cat CatData { get; private set; }
    [SerializeField] private SpriteRenderer catSr;

    public void InitCat(Vector3 initPos, int spriteIdx)
    {
        transform.localPosition = initPos;
        catSr.sprite = DiningManager.Instance.catSprites[spriteIdx];
    }
}

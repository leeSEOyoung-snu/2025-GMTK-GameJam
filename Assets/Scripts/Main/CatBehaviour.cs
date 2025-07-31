using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat
{
    
}

public class CatBehaviour : MonoBehaviour
{
    public Cat CatData { get; private set; }

    public void InitCat(Vector3 initPos)
    {
        transform.localPosition = initPos;
    }
}

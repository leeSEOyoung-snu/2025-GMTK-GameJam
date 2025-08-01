using DG.Tweening;
using UnityEngine;

public class Dish
{
    public SushiTypes Sushi;
    public DishTypes Color;
    public Vector3 CurrPos;
}

public class DishBehaviour : MonoBehaviour
{
    [SerializeField] private SpriteRenderer dishSr;
    [SerializeField] private SpriteRenderer sushiSr;
    public Dish DishData { get; private set; }

    private Sequence _rotateSq;

    public void InitDish(SushiTypes sushi, DishTypes color, Vector3 initPos)
    {
        DishData = new Dish();
        DishData.CurrPos = initPos;
        transform.localPosition = DishData.CurrPos;
        
        ChangeSushiType(sushi);
        ChangeDishType(color);
    }

    public void Rotate(Vector3 endPos, bool moveXFirst)
    {
        if (_rotateSq != null && _rotateSq.IsActive() && _rotateSq.IsPlaying())
        {
            _rotateSq.Complete();
            _rotateSq = null;
        }
        _rotateSq = DOTween.Sequence();

        float rotateSpeed = GameManager.Instance.RotateDuration * MainSceneManager.Instance.RotateSpeedFactor;
        
        if (endPos.y.Equals(DishData.CurrPos.y))
            _rotateSq.Append(transform.DOLocalMoveX(endPos.x, rotateSpeed));
        else
        {
            if (moveXFirst)
            {
                Vector3[] path = { new (endPos.x, DishData.CurrPos.y, 0f), new (endPos.x, endPos.y, 0f) };
                _rotateSq.Append(transform.DOLocalPath(path, rotateSpeed, PathType.Linear));
            }
            else
            {
                Vector3[] path = { new (DishData.CurrPos.x, endPos.y, 0f), new (endPos.x, endPos.y, 0f) };
                _rotateSq.Append(transform.DOLocalPath(path, rotateSpeed, PathType.Linear));
            }
        } 

        _rotateSq.Play().OnComplete(TableManager.Instance.CheckDishCondition);
        DishData.CurrPos = endPos;
    }

    public void Eat()
    {
        // TODO: Eat 구현
    }

    public void ChangeDishType(DishTypes color)
    {
        DishData.Color = color;
        dishSr.sprite = TableManager.Instance.dishSprites[(int)color];
    }

    public void ChangeSushiType(SushiTypes sushi)
    {
        DishData.Sushi = sushi;
        if (sushi == SushiTypes.Empty) sushiSr.color = Color.clear;
        else
        {
            sushiSr.color = Color.white;
            sushiSr.sprite = TableManager.Instance.sushiSprites[(int)sushi];
        }
    }
}

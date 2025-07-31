using DG.Tweening;
using UnityEngine;

public enum SushiTypes
{
    Empty,
    Egg,
    Shrimp
}

public class Dish
{
    public SushiTypes Sushi;
    public Vector3 CurrPos;
}

public class DishBehaviour : MonoBehaviour
{
    public Dish DishData { get; private set; }

    private Sequence _rotateSq;

    public void InitDish(SushiTypes sushi, Vector3 initPos)
    {
        DishData = new Dish();
        DishData.Sushi = sushi;
        DishData.CurrPos = initPos;
        transform.localPosition = DishData.CurrPos;
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
                Vector3[] path = new Vector3[] { new Vector3(endPos.x, DishData.CurrPos.y, 0f), new Vector3(endPos.x, endPos.y, 0f) };
                _rotateSq.Append(transform.DOLocalPath(path, rotateSpeed, PathType.Linear));
            }
            else
            {
                Vector3[] path = new Vector3[] { new Vector3(DishData.CurrPos.x, endPos.y, 0f), new Vector3(endPos.x, endPos.y, 0f) };
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
}

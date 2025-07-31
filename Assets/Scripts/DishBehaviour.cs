using DG.Tweening;
using UnityEngine;

public enum SushiTypes
{
    Egg,
    Shrimp
}

public class DishBehaviour : MonoBehaviour
{
    public SushiTypes Sushi { get; private set; }
    public Vector3 CurrPos { get; private set; }

    private Sequence _rotateSq;
    
    // TODO: Rotation 길이 GameManager로 이양
    private readonly float rotateDuration = 1f;

    public void InitDish(SushiTypes sushi, Vector3 initPos)
    {
        Sushi = sushi;
        CurrPos = initPos;
        transform.localPosition = CurrPos;
    }

    public void Rotate(Vector3 endPos, bool moveXFirst = false)
    {
        if (_rotateSq != null && _rotateSq.IsActive())
        {
            _rotateSq.Complete();
            _rotateSq = null;
        }
        _rotateSq = DOTween.Sequence();
        
        if (endPos.y.Equals(CurrPos.y))
            _rotateSq.Append(transform.DOLocalMoveX(endPos.x, rotateDuration));
        else
        {
            if (moveXFirst)
            {
                _rotateSq.Append(transform.DOLocalMoveX(endPos.x, rotateDuration / 2));
                _rotateSq.Append(transform.DOLocalMoveY(endPos.y, rotateDuration / 2));
            }
            else
            {
                _rotateSq.Append(transform.DOLocalMoveY(endPos.y, rotateDuration / 2));
                _rotateSq.Append(transform.DOLocalMoveX(endPos.x, rotateDuration / 2));
            }
        } 

        _rotateSq.Play();
        CurrPos = endPos;
    }
}

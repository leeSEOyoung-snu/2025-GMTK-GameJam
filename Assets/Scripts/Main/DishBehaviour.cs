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

    public void InitDish(SushiTypes sushi, Vector3 initPos)
    {
        Sushi = sushi;
        CurrPos = initPos;
        transform.localPosition = CurrPos;
    }

    public void Rotate(Vector3 endPos, bool moveXFirst = false)
    {
        if (_rotateSq != null && _rotateSq.IsActive() && _rotateSq.IsPlaying())
        {
            _rotateSq.Complete();
            _rotateSq = null;
        }
        _rotateSq = DOTween.Sequence();

        float rotateSpeed = GameManager.Instance.RotateDuration * MainSceneManager.Instance.RotateSpeedFactor;
        
        if (endPos.y.Equals(CurrPos.y))
            _rotateSq.Append(transform.DOLocalMoveX(endPos.x, rotateSpeed));
        else
        {
            if (moveXFirst)
            {
                Vector3[] path = new Vector3[] { new Vector3(endPos.x, CurrPos.y, 0f), new Vector3(endPos.x, endPos.y, 0f) };
                _rotateSq.Append(transform.DOLocalPath(path, rotateSpeed, PathType.Linear));
            }
            else
            {
                Vector3[] path = new Vector3[] { new Vector3(CurrPos.x, endPos.y, 0f), new Vector3(endPos.x, endPos.y, 0f) };
                _rotateSq.Append(transform.DOLocalPath(path, rotateSpeed, PathType.Linear));
            }
        } 

        _rotateSq.Play();
        CurrPos = endPos;
    }
}

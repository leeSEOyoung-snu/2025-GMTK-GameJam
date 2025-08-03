using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dish
{
    public SushiTypes Sushi;
    public ColorTypes Color;
    public Vector3 CurrPos;
}

public class DishBehaviour : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum MoveMethod
    {
        Rotate,
        GenerateSushi,
        SwapPosition,
        EmptyMotion,
        ChangeMotion,
    }

    private MoveMethod onCompleteMethod;
    private SushiTypes onCompleteSushi;
    private bool onCompletedBool;
    private Vector3 onCompleteVec3;
    
    [SerializeField] private SpriteRenderer dishSr;
    [SerializeField] private SpriteRenderer sushiSr;
    public Dish DishData { get; private set; }

    private Sequence _rotateSq;
    private Vector3 _hoveredPos;

    private readonly float _hoveredPosY = 0.5f, _swapDurationFactor = 1.4f;

    private bool isSelected;

    public void InitDish(SushiTypes sushi, ColorTypes color, Vector3 initPos)
    {
        DishData = new Dish();
        DishData.CurrPos = initPos;
        transform.localPosition = DishData.CurrPos;
        _hoveredPos = new Vector3(initPos.x, initPos.y + _hoveredPosY, initPos.z);
        isSelected = false;
        
        ChangeSushiType(sushi);
        ChangeDishType(color);
    }
    
    public void Rotate(Vector3 endPos, bool moveXFirst)
    {
        if (_rotateSq != null && _rotateSq.IsActive() && _rotateSq.IsPlaying())
        {
            StartCoroutine(WaitForSequenceAndDoNext(MoveMethod.Rotate, endPos, SushiTypes.SushiStandBy, moveXFirst));
            return;
        }
        
        // if (_rotateSq != null && _rotateSq.IsActive() && _rotateSq.IsPlaying())
        // {
        //     _rotateSq.Kill(true);
        //     _rotateSq = null;
        // }
        
        _hoveredPos = new Vector3(endPos.x, endPos.y + _hoveredPosY, endPos.z);
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
    
    public void ChangeDishType(ColorTypes color)
    {
        DishData.Color = color;
        dishSr.sprite = TableManager.Instance.dishSprites[(int)color];
    }

    public void ChangeSushiType(SushiTypes sushi)
    {
        DishData.Sushi = sushi;
        if (sushi == SushiTypes.SushiEmpty)
        {
            sushiSr.color = Color.clear;
        }
        else
        {
            sushiSr.color = Color.white;
            sushiSr.sprite = TableManager.Instance.sushiSprites[(int)sushi];
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (MainSceneManager.Instance.CookStarted)
        {
            if (DishData.Sushi != SushiTypes.SushiEmpty) return;
            CardManager.Instance.PutSushiOnDish(this);
        }
        else
        {
            if (isSelected)
            {
                isSelected = false;
                TableManager.Instance.DishDeSelected();
                OnPointerExit(null);
            }
            else
            {
                isSelected = true;
                TableManager.Instance.DishSelected(this);
                OnPointerEnter(null);
            }
        }
    }

    public void PutSushiOnDish(SushiTypes sushi)
    {
        DishData.Sushi = sushi;
        if (sushi == SushiTypes.SushiEmpty)
        {
            sushiSr.color = Color.clear;
        }
        else
        {
            sushiSr.sprite = TableManager.Instance.sushiSprites[(int)sushi];
            sushiSr.color = Color.white;
        }
        OnPointerExit(null);
    }

    public void GenerateSushi(SushiTypes sushi)
    {
        if (_rotateSq != null && _rotateSq.IsActive() && _rotateSq.IsPlaying())
        {
            StartCoroutine(WaitForSequenceAndDoNext(MoveMethod.GenerateSushi, Vector3.zero, sushi, false));
            return;
        }
        
        ChangeSushiType(sushi);
        
        // if (_rotateSq != null && _rotateSq.IsActive() && _rotateSq.IsPlaying())
        // {
        //     _rotateSq.Kill(true);
        // }
        _rotateSq = DOTween.Sequence();
        Vector3[] path = { new Vector3(sushiSr.transform.localPosition.x, sushiSr.transform.localPosition.y + _hoveredPosY, sushiSr.transform.localPosition.z), sushiSr.transform.localPosition };
        _rotateSq.Append(sushiSr.transform.DOLocalPath(path, CardManager.Instance.CardMoveDuration));
        _rotateSq.Play().OnComplete( () => { TableManager.Instance.GenerateSushiCompleted(sushi); });
    }

    public void SwapPosition(Vector3 endPos)
    {
        if (_rotateSq != null && _rotateSq.IsActive() && _rotateSq.IsPlaying())
        {
            StartCoroutine(WaitForSequenceAndDoNext(MoveMethod.SwapPosition, endPos, SushiTypes.SushiStandBy, false));
            return;
        }
        // if (_rotateSq != null && _rotateSq.IsActive() && _rotateSq.IsPlaying())
        // {
        //     _rotateSq.Kill(true);
        // }
        _rotateSq = DOTween.Sequence();
        Vector3[] path = new Vector3[3] { _hoveredPos, Vector3.zero, Vector3.zero };
        DishData.CurrPos = endPos;
        _hoveredPos = new Vector3(DishData.CurrPos.x, DishData.CurrPos.y + _hoveredPosY, DishData.CurrPos.z);
        path[1] = _hoveredPos; path[2] = endPos;
        _rotateSq.Append(transform.DOLocalPath(path, CardManager.Instance.CardMoveDuration * _swapDurationFactor));
        _rotateSq.Play().OnComplete(EndSwap);
    }

    private void EndSwap()
    {
        isSelected = false;
    }

    public void ReadyToCook()
    { 
        isSelected = false;
        OnPointerExit(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MainSceneManager.Instance.isRotating || isSelected || DishData.Sushi != SushiTypes.SushiEmpty) return;
        if (_rotateSq != null && _rotateSq.IsActive() && _rotateSq.IsPlaying())
        {
            _rotateSq.Kill(true);
        }
        _rotateSq = DOTween.Sequence();
        _rotateSq.Append(transform.DOLocalMove(_hoveredPos, CardManager.Instance.CardMoveDuration));
        _rotateSq.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (MainSceneManager.Instance.isRotating || isSelected) return;
        if (_rotateSq != null && _rotateSq.IsActive() && _rotateSq.IsPlaying())
        {
            _rotateSq.Kill(true);
        }
        _rotateSq = DOTween.Sequence();
        _rotateSq.Append(transform.DOLocalMove(DishData.CurrPos, CardManager.Instance.CardMoveDuration));
        _rotateSq.Play();
    }

    public void EmptyMotion()
    {
        if (_rotateSq != null && _rotateSq.IsActive() && _rotateSq.IsPlaying())
        {
            StartCoroutine(WaitForSequenceAndDoNext(MoveMethod.EmptyMotion, Vector3.zero, SushiTypes.SushiStandBy, false));
            return;
        }
        // if (_rotateSq != null && _rotateSq.IsActive() && _rotateSq.IsPlaying())
        // {
        //     _rotateSq.Kill(true);
        // }
        _rotateSq = DOTween.Sequence();
        Vector3[] path = { new Vector3(sushiSr.transform.localPosition.x, sushiSr.transform.localPosition.y + _hoveredPosY, sushiSr.transform.localPosition.z), sushiSr.transform.localPosition };
        _rotateSq.Append(sushiSr.transform.DOLocalPath(path, CardManager.Instance.CardMoveDuration));
        _rotateSq.Play().OnComplete(SetSushiEmpty);
    }

    public void SetSushiEmpty()
    {
        ChangeSushiType(SushiTypes.SushiEmpty);
        InteractionManager.Instance.ActivateResult();
    }
    
    public void ChangeMotion(SushiTypes sushi)
    {
        if (_rotateSq != null && _rotateSq.IsActive() && _rotateSq.IsPlaying())
        {
            StartCoroutine(WaitForSequenceAndDoNext(MoveMethod.ChangeMotion, Vector3.zero, sushi, false));
            return;
        }
        // if (_rotateSq != null && _rotateSq.IsActive() && _rotateSq.IsPlaying())
        // {
        //     _rotateSq.Kill(true);
        // }
        _rotateSq = DOTween.Sequence();
        Vector3[] path = { new Vector3(sushiSr.transform.localPosition.x, sushiSr.transform.localPosition.y + _hoveredPosY, sushiSr.transform.localPosition.z), sushiSr.transform.localPosition };
        _rotateSq.Append(sushiSr.transform.DOLocalPath(path, CardManager.Instance.CardMoveDuration));
        _rotateSq.Play().OnComplete(() => { SetSushiChanged(sushi); });
    }

    public void SetSushiChanged(SushiTypes sushi)
    {
        ChangeSushiType(sushi);
        TableManager.Instance.EndChangeType();
    }

    private IEnumerator WaitForSequenceAndDoNext(MoveMethod method, Vector3 vec, SushiTypes sushi, bool boolean)
    {
        onCompleteMethod = method;
        onCompleteSushi = sushi;
        onCompleteVec3 = vec;
        onCompletedBool = boolean;
        yield return _rotateSq.WaitForCompletion();

        switch (method)
        {
            case MoveMethod.Rotate:
                Rotate(onCompleteVec3, onCompletedBool);
                break;
            
            case MoveMethod.GenerateSushi:
                GenerateSushi(onCompleteSushi);
                break;
            
            case MoveMethod.SwapPosition:
                SwapPosition(onCompleteVec3);
                break;
            
            case MoveMethod.EmptyMotion:
                EmptyMotion();
                break;
            
            case MoveMethod.ChangeMotion:
                ChangeMotion(onCompleteSushi);
                break;
        }
    }
}

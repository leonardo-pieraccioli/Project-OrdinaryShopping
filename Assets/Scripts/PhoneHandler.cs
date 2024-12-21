using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class PhoneHandler : MonoBehaviour
{

    private float _startY = -180;
    private float _finalY = 200;
    private enum State{
        rising,
        descending,
        bottom,
        top,
    };
    private State _state = State.bottom;
    private List<RectTransform> _rectTrans = new List<RectTransform>();
    [SerializeField] private float _phoneMoveSpeed = 10;

    void Start()
    {
        foreach(RectTransform rt in transform.GetComponentInChildren<RectTransform>())
        {
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, _startY);
            Debug.Log(rt.anchoredPosition);
            _rectTrans.Add(rt);
        }
        Debug.Assert(_rectTrans.Count == 2, $"Incorrect number of Rect Transform found: {_rectTrans.Count}.\nIt should be 2.");
    }

    void FixedUpdate()
    {

        if(_state == State.bottom || _state == State.top)
        {
            return;
        }

        if(_state ==State.rising)
        {
            foreach(RectTransform rt in _rectTrans) 
            {
                if(rt.anchoredPosition.y < _finalY)
                {
                    if(_finalY - rt.anchoredPosition.y < _phoneMoveSpeed)
                    {
                        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, _finalY);
                    }
                    else
                    {
                        rt.anchoredPosition += new Vector2(0, _phoneMoveSpeed);
                    }
                }
            }
        }
        else
        {
            foreach(RectTransform rt in _rectTrans) 
            {
                if(rt.anchoredPosition.y > _startY)
                {
                    if(rt.anchoredPosition.y - _startY < _phoneMoveSpeed)
                    {
                        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, _startY);
                    }
                    else
                    {
                        rt.anchoredPosition += new Vector2(0, -_phoneMoveSpeed);
                    }
                }
            }
        }

    }

    void Update()
    {
        if(Input.GetKeyDown(GameKeys.SHOW_PHONE_KEY))
        {
            switch(_state)
            {
                case State.rising:
                _state = State.descending;
                break;
                case State.descending:
                _state = State.rising;
                break;
                case State.bottom:
                _state = State.rising;
                break;
                case State.top:
                _state = State.descending;
                break;
            }
        }
    }

}

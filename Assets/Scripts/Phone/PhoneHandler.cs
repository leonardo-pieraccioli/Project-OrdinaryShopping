using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    private Transform _bodyTrans;
    private RectTransform _rectTransBody;
    [SerializeField] private float _phoneMoveSpeed = 10;
    private Queue<GameObject> _screens = new Queue<GameObject>();
    private GameObject _currentScreen;


    void Start()
    {
        _bodyTrans = transform.GetChild(0);
        Debug.Assert(_bodyTrans != null, "Screen body transform not found");
        _rectTransBody = _bodyTrans.GetComponent<RectTransform>();
        Debug.Assert(_rectTransBody != null, "Screen body rect transform not found");

        _rectTransBody.anchoredPosition = new Vector2(_rectTransBody.anchoredPosition.x, _startY);
        Debug.Assert(_rectTransBody != null);


        for(int i = 0; i < _bodyTrans.childCount; i++)
        {
            GameObject c = _bodyTrans.GetChild(i).GameObject();
            if(c.tag == "Screen")
            {
                c.SetActive(false);
                _screens.Enqueue(c);
            }
            if(c.tag == "ScreenDefault")
            {
                c.SetActive(true);
                _currentScreen = c;
            }
        }
        Debug.Assert(_screens.Count == 2 - 1, "Wrong number of screens");
    }

    void FixedUpdate()
    {

        if(_state == State.bottom || _state == State.top)
        {
            return;
        }

        if(_state ==State.rising)
        {

            if(_rectTransBody.anchoredPosition.y < _finalY)
            {
                if(_finalY - _rectTransBody.anchoredPosition.y < _phoneMoveSpeed)
                {
                    _rectTransBody.anchoredPosition = new Vector2(_rectTransBody.anchoredPosition.x, _finalY);
                }
                else
                {
                    _rectTransBody.anchoredPosition += new Vector2(0, _phoneMoveSpeed);
                }
            }

        }
        else
        {

            if(_rectTransBody.anchoredPosition.y > _startY)
            {
                if(_rectTransBody.anchoredPosition.y - _startY < _phoneMoveSpeed)
                {
                    _rectTransBody.anchoredPosition = new Vector2(_rectTransBody.anchoredPosition.x, _startY);
                }
                else
                {
                    _rectTransBody.anchoredPosition += new Vector2(0, -_phoneMoveSpeed);
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

        if(Input.GetKeyDown(GameKeys.SWITCH_PHONE_APP))
        {
            ChangeAppPage();
        }
    }

    private void ChangeAppPage() 
    {
        GameObject nextScreen = _screens.Dequeue();
        nextScreen.SetActive(true);
        _currentScreen.SetActive(false);
        _screens.Enqueue(_currentScreen);
        _currentScreen = nextScreen;
    }

}

using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private Transform _bodyTrans;
    private RectTransform _rectTransBody;
    [SerializeField] private float _phoneMoveSpeed = 10;
    private LinkedList<GameObject> _screens = new LinkedList<GameObject>();
    private GameObject _currentScreen;

    void Start()
    {
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
            }
            if(c.tag == "ScreenDefault")
            {
                _currentScreen = c;
                c.SetActive(true);
            }
            _screens.AddLast(c);
        }
        // Debug.Assert(_screens.Count == 2 - 1, "Wrong number of screens");
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
            else
            {
                _state = State.top;
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
            else
            {
                _state = State.bottom;
            }

        }

    }

    public void OnTakePhone(InputValue input)
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

    public void OnPhonePageLeft(InputValue input)
    {
        if(_state == State.top)
            OpenLeftPage();
    }

    public void OnPhonePageRight(InputValue input)
    {
        if(_state == State.top)
            OpenRightPage();
    }

    void Update()
    {

    }

    private void OpenLeftPage() 
    {
        var previousScreen = _screens.Find(_currentScreen).Previous;
        if (previousScreen == null)
        {
            previousScreen = _screens.Last;
        }
        GameObject newScreen = previousScreen.Value;
        newScreen.SetActive(true);
        _currentScreen.SetActive(false);
        _currentScreen = newScreen;
    }

    private void OpenRightPage()
    {
        var nextScreen = _screens.Find(_currentScreen).Next;
        if (nextScreen == null)
        {
            nextScreen = _screens.First;
        }
        GameObject newScreen = nextScreen.Value;
        newScreen.SetActive(true);
        _currentScreen.SetActive(false);
        _currentScreen = newScreen;
    }

}

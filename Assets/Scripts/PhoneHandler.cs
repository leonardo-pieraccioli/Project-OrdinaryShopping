using System.Collections.Generic;
using UnityEngine;

public class PhoneHandler : MonoBehaviour
{

    private float _startY = -180;
    private float _finalY = 200;
    [SerializeField] private float _phoneMoveSpeed = 10;
    private List<RectTransform> _rectTrans = new List<RectTransform>();
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
        //phone pop up
        if(Input.GetKey(GameKeys.SHOW_PHONE_KEY))
        {
            foreach(RectTransform rt in _rectTrans) 
            {
                if(rt.anchoredPosition.y < _finalY)
                {
                    rt.anchoredPosition += new Vector2(0, _phoneMoveSpeed);
                }
            }
        }
        //phone pop out
        else
        {
            foreach(RectTransform rt in _rectTrans) 
            {
                if(rt.anchoredPosition.y > _startY)
                {
                    rt.anchoredPosition += new Vector2(0, -_phoneMoveSpeed);
                }
            }
        }
    }
}

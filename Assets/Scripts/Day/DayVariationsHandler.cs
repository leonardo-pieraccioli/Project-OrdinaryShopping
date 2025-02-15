using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayVariationsHandler : MonoBehaviour
{
    [SerializeField] private int dayN = 0;

    public int getDayNumber()
    {
        return dayN;
    }

    public void enable()
    {
        gameObject.SetActive(true);
    }

    
    public void disable()
    {
        gameObject.SetActive(false);
    }
}

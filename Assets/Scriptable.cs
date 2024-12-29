using UnityEngine;

public class DayInfo : ScriptableObject
{
    // Class that represents a specific type of vehicle
    [Range(0.1f, 100f)]
    public float m_MaxSpeed = 0.1f;

    [Range(0.1f, 10f)]
    public float m_MaxAcceration = 0.1f;
    public string str = "Day";
    // This class could have many other vehicle parameters, such as Turning Radius, Range, Damage etc
}
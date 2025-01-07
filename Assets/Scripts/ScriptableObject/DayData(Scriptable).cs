using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDayData", menuName = "ScriptableObjects/DayData", order = 1)]
public class DayData : ScriptableObject
{
    public int dayNumber;       // Numero del giorno
    public int numberOfCubes;   // Numero di cubi da generare
    public Color cubeColor;     // Colore dei cubi


}

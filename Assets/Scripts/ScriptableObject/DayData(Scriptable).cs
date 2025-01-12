using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDayData", menuName = "ScriptableObjects/DayData", order = 1)]
public class DayData : ScriptableObject
{
    public int dayNumber;       // Numero del giorno
    public float budget;        //Budget Giornaliero

    //stato di salute(malattia,freddo,disidratazione,stanchezza...)
    //atmosfera(luci,musica,bombe)
    //diario
    //telefono
    //npc
    //lista spesa
    //news
    public  Shader shader;
    //prodotti
    public Product[] products;
    
   


}

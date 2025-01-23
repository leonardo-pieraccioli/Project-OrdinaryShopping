using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//questo script deve assegnare dinamicamente i giorni scriptable ai vari Manager, cioè Day1, Day2 ecc
public class DayManager : MonoBehaviour
{
   // public DayData dayData;     
    void Start()
    {     
        //bisogna prendere lo scriptable da caricare e caricarlo nei manager
        GetDay("Day1");
    }


//assegna a tutti i Manager il giorno giusto
    public void GetDay(string nameScriptable){
        LoadDayData(nameScriptable);
        //qui assegno il dayData giusto usando il Singelton
        PositionScript.dayData=ScriptableObjectManager.Instance.CurrentDayData; 
    }

    public void LoadDayData(string nameScriptable){
        //if day=1
        DayData loadedDayData=AssetDatabase.LoadAssetAtPath<DayData>("Assets/ScriptableObject/"+nameScriptable+".asset");
        //assegno il manager
        if (loadedDayData != null)
        {
            // Assegna lo ScriptableObject al manager
            ScriptableObjectManager.Instance.CurrentDayData = loadedDayData;
            //qui io ho assegnato semplicemente allo spawner il manager giusto non il day
            Debug.Log("Caricato GameData: " + loadedDayData.name);
        }
        else
        {
            Debug.LogError("GameData non trovato: " + nameScriptable);
        }

    }

}
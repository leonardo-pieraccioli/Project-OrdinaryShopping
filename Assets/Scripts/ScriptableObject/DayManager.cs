using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//questo script deve assegnare dinamicamente i giorni scriptable ai vari Manager
public class DayManager : MonoBehaviour
{
   // public DayData dayData;     
    public static DayData Instance { get; private set; }
    void Start()
    {
        
        LoadDayData("Day1");
              //bisogna prendere lo scriptable da caricare e caricarlo nei manager
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
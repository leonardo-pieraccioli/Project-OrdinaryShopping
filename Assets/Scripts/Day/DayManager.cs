using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//questo script deve assegnare dinamicamente i giorni scriptable ai vari Manager, cioè Day1, Day2 ecc
public class DayManager : MonoBehaviour
{
    public DayData[] listDayData;
    public DayData currentDay;
    private static DayManager _instance;
    public static DayManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<DayManager>();
            }

            return _instance;
        }
    }    
    void Start()
    {     
        //bisogna prendere lo scriptable da caricare e caricarlo nei manager
        LoadDayData(1);
    }

    public void LoadDayData(int nameScriptable){
        
        DayData loadedDayData=listDayData[nameScriptable-1];
        currentDay=loadedDayData;
        if (loadedDayData != null)
        { 
           
            //init NPC
            DialogueManager.Instance.Init(loadedDayData.npc); 

            //init Product
            ProductManager.Instance.Init(loadedDayData.productInfo);
           
            //init Light
            LightManager.Instance.Init(loadedDayData.light);
           
            //init shopping list
            GroceriesList.Instance.Init(loadedDayData.productInfo);

            //init balance
            BalanceText.Instance.SetBalance(loadedDayData.budget);
            
            Debug.Log("Caricato Day" + nameScriptable++);
        }
        else
        {
            Debug.LogError("GameData non trovato: " + nameScriptable);
        }

    }

}
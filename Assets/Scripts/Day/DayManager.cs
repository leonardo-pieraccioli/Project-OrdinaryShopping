using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

//questo script deve assegnare dinamicamente i giorni scriptable ai vari Manager, cioè Day1, Day2 ecc
public class DayManager : MonoBehaviour
{
    private List<DayVariationsHandler> dayVar;
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
        //carica gli empty delle variazioni 
        dayVar = FindObjectsOfType<DayVariationsHandler>().ToList();
        //bisogna prendere lo scriptable da caricare e caricarlo nei manager
        LoadDayData(1); 
    }

    private void SaturationHandler()
    {
        //Gestione Saturazione globale
        float value = -100.0f * Mathf.Pow(currentDay.dayNumber/3, 2) / 9.0f;
        GameObject postProcess = GameObject.Find("PostProcess");
        Volume v = postProcess.GetComponent<Volume>();
        ColorAdjustments ca;
        if(v.profile.TryGet<ColorAdjustments>(out ca))
            ca.saturation.value = value;
    }

    //Chiamata ogni giorno
    public void LoadDayData(int nameScriptable){


        DayData loadedDayData=listDayData[nameScriptable-1];
        currentDay=loadedDayData;

        Debug.Log($"Current day is {currentDay.dayNumber}");

        foreach(DayVariationsHandler v in dayVar)
        {
            if(v.getDayNumber() == currentDay.dayNumber)
            {
                v.enable();
            }
            else
            {
                v.disable();
            }
        }
        
        //Gestione Frutta
        if(currentDay.dayNumber == 5)
        {
            Destroy(GameObject.Find("DeletedFruits"));
        }


        if (loadedDayData != null)
        { 
           
            //init NPC
            DialogueManager.Instance.Init(loadedDayData.npc); 

            //init Product
            ProductManager.Instance.Init(loadedDayData.productInfo.products,nameScriptable);

            //init Light
            LightManager.Instance.Init(loadedDayData.light);
           
            //init shopping list
            GroceriesList.Instance.Init(loadedDayData.productInfo);

            //init balance
            BalanceText.Instance.SetBalance(loadedDayData.budget);

            DiaryManager.Instance.Init(loadedDayData.diaryDay);
            
            Debug.Log("Caricato Day" + nameScriptable++);
        }
        else
        {
            Debug.LogError("GameData non trovato: " + nameScriptable);
        }

    }

}
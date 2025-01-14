using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectManager : MonoBehaviour
{
   
    /*
    Instance è una proprietà statica di una classe, 
    utilizzata per implementare un singleton(design pattern). 
    Un singletongarantisce che esista una sola istanza di una classe
     e fornisce un punto di accesso globale a quella istanza.*/
     //variabile di classe
    public static ScriptableObjectManager Instance { get; private set; }
    //private set significa che solo la classe ScriptableObjectManager può impostare il valore di Instance
    //puoi però accedere a Instance da qualsiasi altro script tramite SciptableObjectManger.Instance  
    
    // Evento per notificare quando lo ScriptableObject cambia
   // public event System.Action<DayData> OnDayDataChanged;

    // Riferimento attuale allo ScriptableObject
    private DayData _currentDayData;

    public DayData CurrentDayData
    {
        get => _currentDayData;
        
        set
        {
            if (_currentDayData != value)
            {
                _currentDayData = value;
               // OnDayDataChanged?.Invoke(_currentDayData); // Notifica il cambiamento
            }
        }
    }


    //qui assegniamo il valore a Instance (solo una volta)
    private void Awake()
    {
        // Implementazione del Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Mantieni il gestore anche quando cambi scena
    }
}

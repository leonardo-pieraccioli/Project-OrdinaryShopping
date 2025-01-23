using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionScript : MonoBehaviour
{
     public static DayData dayData; 
    // Riferimento allo ScriptableObject DayData
    //Un riferimento a un ScriptableObject che sarà modificato 
    //(in particolare, verrà aggiornato un suo campo con i dati delle posizioni).
    //[SerializeField] DayData _objectToChange;
    /*Un array di Vector3 usato per memorizzare le posizioni dei GameObject*/
    [SerializeField] Vector3[] storedPositions; //Trasform
    /*Array di GameObject di cui verranno estratte le posizioni.*/
    [SerializeField] GameObject[] _gameObjectsRef;
   
    //Tenere traccia degli oggetti istanziati e risorse allocate
    public static Dictionary<string,(GameObject instance, Material material)> instantiatedProducts = new Dictionary<string, (GameObject, Material)>();
    
    [ContextMenu("Store GameObjects Positions for SO")]

    void Start()
    {
        //posizioni oggetti
        //aggiorna vettore positions di dayData
        //salva la posizione degli oggetti nell array dentro lo scriptable object dayData
        StoreGameObjectPositionsIntoSO();

        //generare vari prodotti
        for(int i=0;i<dayData.products.Length;i++){
           
            Product.GenerateBlock(dayData.products[i],_gameObjectsRef[i],dayData.shader,1,2,1,new Vector3(0,0,0));
        }

        
    }
void StoreGameObjectPositionsIntoSO()
    {
        if (_gameObjectsRef.Length > 0)// Controlla che ci siano GameObject nell'array
        {
            /*Usa reflection per accedere al campo _positions all'interno dell'_objectToChange (che è il ScriptableObject). Questo campo deve essere definito nel 
            ScriptableObject, altrimenti restituirà null.*/
            
            //System.Reflection.FieldInfo product = _objectToChange.GetType().GetField("product");
            //System.Reflection.FieldInfo _positions=_objectToChange.products.GetType().GetField("_positions");
            /*Inizializza un array storedPositions con una dimensione pari al numero di GameObject referenziati.*/
            
            storedPositions = new Vector3[_gameObjectsRef.Length];
            /*Itera attraverso l'array di GameObject e memorizza la posizione di ciascuno di essi nell'array storedPositions.*/
            for (int i = 0; i < _gameObjectsRef.Length; i++)
            {
                storedPositions[i] = _gameObjectsRef[i].transform.position;

            }
            /*Se il campo _positions esiste nel ScriptableObject, lo aggiorna con il valore di storedPositions usando reflection.
Se _positions non esiste, viene stampato un messaggio di errore nella console.
*/
            for (int i = 0; i < _gameObjectsRef.Length; i++)
            {
                   dayData.products[i]._positions=storedPositions[i];

            }
         
            
           /*if (_positions != null)
           {
                _objectToChange.products.GetType().GetField("_positions").SetValue(_objectToChange, storedPositions);
            }
            else
            {
                Debug.Log("_positions variable does not exist in this SO : " + _objectToChange.name);
            }*/
        }
    }
   



}
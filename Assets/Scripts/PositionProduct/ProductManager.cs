using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class ProductManager : MonoBehaviour
{
    public static DayData dayData;
    public static Productinfo[] products;
    // Riferimento allo ScriptableObject DayData
    //Un riferimento a un ScriptableObject che sarà modificato 
    //(in particolare, verrà aggiornato un suo campo con i dati delle posizioni).
    //[SerializeField] DayData _objectToChange;
    /*Un array di Vector3 usato per memorizzare le posizioni dei GameObject*/
    [SerializeField] Vector3[] storedPositions; //Trasform
    /*Array di GameObject di cui verranno estratte le posizioni.*/
    [SerializeField] GameObject[] _gameObjectsRef;

    //Tenere traccia degli oggetti istanziati e risorse allocate
    public static Dictionary<string, (GameObject instance, Material material)> instantiatedProducts = new Dictionary<string, (GameObject, Material)>();

    private static ProductManager _instance;
    public static ProductManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ProductManager>();
            }

            return _instance;
        }
    }

    [ContextMenu("Store GameObjects Positions for SO")]

    public void Init(DayData currentdaydata)
    {
        if (currentdaydata == null)
        {
            Debug.LogError("Errore: currentdaydata è nullo");
            return;
        }

        dayData = currentdaydata;

        // Controllo se l'array products esiste prima di copiarlo
        if (currentdaydata.products != null && currentdaydata.products.Length > 0)
        {
            
            // Inizializza products con la dimensione corretta
            products = new Productinfo[currentdaydata.products.Length];

            // Copia l'array senza usare il ciclo for
            Array.Copy(currentdaydata.products, products, currentdaydata.products.Length);
        }
        else
        {
            Debug.LogWarning("Attenzione: l'array products è nullo o vuoto.");
            products = new Productinfo[0]; // Inizializza a un array vuoto per evitare NullReferenceException
        }

        //Product.DestroyAll();

        StoreGameObjectPositionsIntoSO();

        //generare vari prodotti
        for (int i = 0; i < products.Length; i++)
        {
            Product.Generate(products[i], _gameObjectsRef[i]);
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
                dayData.products[i]._positions = storedPositions[i];

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
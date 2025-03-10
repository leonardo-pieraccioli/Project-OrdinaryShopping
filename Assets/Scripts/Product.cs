using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine.InputSystem.LowLevel;

[System.Serializable]
public class Productinfo
{
    public String LabelPosition;
    public string productName; // Nome del prodotto
    public GameObject prefabs; //prefab prodotto
    public float price; // Prezzo del prodotto
    public string description; // Descrizione del prodotto
    public bool isInShoppingList;
    
    public Vector3 sizeCollider;
    public Vector3 centerCollider;
    public GameObject emptyPos;
/*
    public Boolean poco;
    public Boolean tanto;
    public Boolean singolo;
*/
    //public int numberOfProduct; 

    public Vector3 _dimensions = new Vector3(1f, 1f, 1f); // Le dimensioni del prodotto (default a 1x1x1)
    
    //posizione
    public Vector3 _positions;
    //il transform è memorizzato nel prefab
    
    public int _xn , _yn, _zn;
    public Vector3 _offset;
    public bool _rotate;
   /*  public int _grabbedCount;
    public bool _grabOne, _putOne; */

}

public class Product: MonoBehaviour{
    //Tenere traccia degli oggetti istanziati e risorse allocate
    public static List<GameObject> activeProduct=new List<GameObject>();
    // DEFINISCO LE FUNZIONI QUI
    static public void Generate(Productinfo product)
{
    if (product.prefabs == null)
    {
        Debug.Log("Prefab non assegnato nell'Inspector!");
        return;
    }
    
    // Istanzia l'oggetto
    GameObject instance = Instantiate(product.prefabs, product._positions, product.prefabs.transform.rotation);
  
    // Ora che l'oggetto è istanziato, possiamo regolare la sua posizione e scala
    instance.transform.localScale = product.prefabs.transform.localScale;
    instance.transform.SetPositionAndRotation(product._positions, product.prefabs.transform.rotation);
    instance.transform.SetParent(product.emptyPos.transform);
    // Aggiorna le dimensioni del prodotto (con la scala effettiva del prefab)
    product._dimensions = instance.transform.lossyScale;
   

    // Imposta il nome
    instance.name = product.productName;
    activeProduct.Add(instance);

    // Aggiungi il componente ArrayInstanceProduct e inizializza
    ArrayInstanceProduct arrayInstances = instance.AddComponent<ArrayInstanceProduct>();

    // Inizializza l'array
    arrayInstances.Init(product);
}




     static private Vector3 GetPrefabDimensions(GameObject instance)
    {
        Renderer renderer = instance.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Usa il Renderer per ottenere le dimensioni (bounding box)
            return renderer.bounds.size;
        }
        
        // Se non c'è un Renderer, prova a usare un Collider
        Collider collider = instance.GetComponent<Collider>();
        if (collider != null)
        {
            // Usa il Collider per ottenere le dimensioni
            return collider.bounds.size;
        }

        // Se non ci sono né Renderer né Collider, ritorna le dimensioni di default
        return new Vector3(1f, 1f, 1f);  // Dimensione di default, ad esempio 1x1x1
    }
       static public void Destroy(){

        // Controlla se l'oggetto esiste
        foreach (GameObject product in activeProduct)
        {
            // Distruggi l'oggetto
            Destroy(product);
            Debug.Log($"Oggetto {product.name} distrutto.");
        }
        
    	activeProduct.Clear();

    }
/* 
    static public void Destroy(Productinfo product){

        //considerare l'idea di salvarsi una lista dei prodotti istanziati e poi su quella chiamare la destroy
        // Trova l'oggetto in scena utilizzando il suo nome
        GameObject oggetto = GameObject.Find(product.productName);

        // Controlla se l'oggetto esiste
        if (oggetto != null)
        {
            // Distruggi l'oggetto
            Destroy(oggetto);
            Debug.Log($"Oggetto {product.productName} distrutto.");
        }
        else
        {
            Debug.LogWarning($"Oggetto con nome {product.productName} non trovato in scena.");
        }


    } */
    
    
}

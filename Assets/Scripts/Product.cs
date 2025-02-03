using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using Unity.VisualScripting;

[System.Serializable]
public class Productinfo
{
    public string productName; // Nome del prodotto
    public float price; // Prezzo del prodotto
    public string description; // Descrizione del prodotto
    public GameObject prefabs; //prefab prodotto

/*
    public Boolean poco;
    public Boolean tanto;
    public Boolean singolo;
*/
    public int numberOfProduct; 

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

    // DEFINISCO LE FUNZIONI QUI
    static public void Generate(Productinfo product, GameObject gameObjectrefer)
    {
            if(product.prefabs==null){
                Debug.LogError("Prefab non assegnati nell'Inspector!");
                return;
            }
            GameObject instance  = Instantiate(product.prefabs,product._positions,product.prefabs.transform.rotation,gameObjectrefer.transform);
            //GameObject currentEntity = Instantiate(product.prefabs, product.prefabs.transform,true);
            //currentEntity.transform.SetLocalPositionAndRotation(product._positions,product.prefabs.transform.rotation);

            //nome prodotto creato
            instance.name=product.productName;
            
            ArrayInstanceProduct arrayInstances=instance.AddComponent<ArrayInstanceProduct>();
            instance.GetComponent<ArrayInstanceProduct>().Init(product);
            
        
    }

     
    static public void Destroy(Productinfo product){

        
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


    }
    
    
}

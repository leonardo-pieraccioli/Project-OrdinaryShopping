using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public DayData dayData; // Riferimento allo ScriptableObject DayData
   
    //int numberOfPrefabs;//viene calcolato randomicamente
    void Start()
    {
        //generare vari prodotti
        for(int i=0;i<dayData.products.Length;i++){
           
            Generate(dayData.products[i]);
            
        }
    }

   
    void Generate(Product product)
    {

            if(product.prefabs==null|| dayData.shader==null){
                Debug.LogError("Prefab o Shader non assegnati nell'Inspector!");
                return;

            }
            GameObject currentEntity = Instantiate(product.prefabs,product._positions,Quaternion.identity);
            //nome prodotto creato
            currentEntity.name=product.productName;

           //Ottieni il MeshRenderer del prefab
        MeshRenderer renderer = currentEntity.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            Debug.LogError("Il prefab non ha un MeshRenderer!");
            return;
        }

            // Crea un nuovo materiale con lo shader desiderato
            Material newMaterial = new Material(dayData.shader);

            // Assegna il nuovo materiale al MeshRenderer del prefab
            renderer.material = newMaterial;

        Debug.Log("Shader assegnato con successo al prefab istanziato.");
        
    
        
    }
}
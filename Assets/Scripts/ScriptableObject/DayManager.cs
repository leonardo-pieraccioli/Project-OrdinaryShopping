using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public DayData dayData; // Riferimento allo ScriptableObject DayData
   
    int numberOfPrefabs;//viene calcolato randomicamente
    void Start()
    {

        //generare vari prodotti
        for(int i=0;i<dayData.products.Length;i++){

            numberOfPrefabs=Random.Range(dayData.minNumberOfProducts,dayData.maxNumberOfProducts);    
            Generate(dayData.products[i].prefabs,numberOfPrefabs,dayData.spawnPointsPrefabs);
            
        }
    }

    void Generate(GameObject prefab, int number, Vector3[] spawnPoints)
    {
        int currentSpawnPointIndex=0;
        int instanceNumber = 1;

        for (int i = 0; i < number; i++)
        {
            
            GameObject currentEntity = Instantiate(prefab,spawnPoints[currentSpawnPointIndex],Quaternion.identity);
            //nome prodotto creato
            currentEntity.name=prefab.name+instanceNumber;

            //posizione oggetti DA MODIFICARE
            currentSpawnPointIndex=(currentSpawnPointIndex+1 )% spawnPoints.Length; 
        
            instanceNumber++;


        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Product
{
    public string productName; // Nome del prodotto
    public float price; // Prezzo del prodotto
    public string description; // Descrizione del prodotto
    public GameObject prefabs; //prefab prodotto

    public Boolean poco;
    public Boolean tanto;
    public Boolean singolo;

    //public int numberOfProduct; 

    //posizione
    public Vector3 _positions;
   

    //SERVONO DELLE FUNZIONI PER LA GESTIONE?
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using Unity.VisualScripting;

public class ProductManager : MonoBehaviour
{
    //----MultipleIstance
    private Mesh _mesh;
    private Material[] _materials;
    private Stack<Matrix4x4> _matrices, _removed;
    
    //-----------------
    public Productinfo[] productInfo;
    private int nistance = 0;

    [SerializeField] Vector3[] storedPositions; // Array per memorizzare le posizioni degli empty

    // Abbiamo rimosso posClass

    int numberShelve;
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
    public void Init(Productinfo[] currentProductInfo, int currentDay)
    {
        if (currentProductInfo == null)
        {
            Debug.LogError("Errore: currentProductInfo è nullo");
            return;
        }
        if (currentDay > 1)
        {
            Product.Destroy();
            // nistance--;
        }

        productInfo = currentProductInfo;
        Array.Copy(currentProductInfo, productInfo, currentProductInfo.Length);

        // Recupera direttamente gli empty e assegna le posizioni
        StoreGameObjectPositionsIntoSO();

        // Genera i vari prodotti
        for (int i = 0; i < currentProductInfo.Length; i++)
        {
            Product.Generate(currentProductInfo[i]);
            nistance++;
        }
    }

    /// <summary>
    /// Raccoglie tutti i GameObject empty.
    /// Si assume che i contenitori siano i figli diretti del GameObject a cui è attaccato il ProductManager,
    /// e che ogni contenitore abbia come figli gli empty da utilizzare.
    /// </summary>
    /// <returns>Un array contenente tutti i GameObject empty trovati.</returns>
    private GameObject[] GetAllEmptyGameObjects()
    {
        List<GameObject> empties = new List<GameObject>();
        int containerCount = transform.childCount;
        for (int i = 0; i < containerCount; i++)
        {
            Transform container = transform.GetChild(i);
            for (int j = 0; j < container.childCount; j++)
            {
                empties.Add(container.GetChild(j).gameObject);
            }
        }
        return empties.ToArray();
    }

    [ContextMenu("Store GameObjects Positions for SO")]
    void StoreGameObjectPositionsIntoSO()
    {
        // Recupera gli empty direttamente dalla gerarchia
        GameObject[] empties = GetAllEmptyGameObjects();
        int totEmpty = empties.Length;
        if (productInfo.Length > totEmpty)
        {
            Debug.Log("Pochi empty nella scena! Dove metto gli oggetti?");
            return;
        }
        storedPositions = new Vector3[totEmpty];
        for (int i = 0; i < empties.Length; i++)
        {
            storedPositions[i] = empties[i].transform.position;
            if (i < productInfo.Length)
            {
                productInfo[i]._positions = empties[i].transform.position;
                productInfo[i].emptyPos = empties[i];
                productInfo[i].LabelPosition = empties[i].name;
            }
            else
            {
                Debug.Log("Pochi prodotti!");
                return;
            }
        }
    }
}

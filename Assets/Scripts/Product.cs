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

}

public class Product: MonoBehaviour{
    Productinfo info;
    // DEFINISCO LE FUNZIONI QUI
    

    // Dizionario per tenere traccia degli oggetti istanziati e delle risorse allocate
    public static Dictionary<string, (GameObject instance, Material material)> instantiatedProducts = new Dictionary<string, (GameObject, Material)>();

    static public void Generate(Productinfo product, GameObject gameObjectrefer)
    {
            if(product.prefabs==null){
                Debug.LogError("Prefab non assegnati nell'Inspector!");
                return;
            }
            GameObject instance  = Instantiate(product.prefabs,product._positions,product.prefabs.transform.rotation,gameObjectrefer.transform);
            // GameObject currentEntity = Instantiate(product.prefabs, product.prefabs.transform,true);
             //currentEntity.transform.SetLocalPositionAndRotation(product._positions,product.prefabs.transform.rotation);

            //nome prodotto creato
            instance .name=product.productName;
              Material newMaterial =null;
            
            //carica mesh giusta
            LoadMesh(instance,product.numberOfProduct);
/*
           //Ottieni il MeshRenderer del prefab
        MeshRenderer renderer = instance.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            Debug.LogError("Il prefab non ha un MeshRenderer!");
            return;
        }

            // Crea un nuovo materiale con lo shader desiderato
             newMaterial = new Material(shader);

            // Assegna il nuovo materiale al MeshRenderer del prefab
            renderer.material = newMaterial;

        Debug.Log("Shader assegnato con successo al prefab istanziato.");   
*/
        // Salva l'oggetto istanziato e il materiale nel dizionario
        if (!instantiatedProducts.ContainsKey(product.productName))
        {
            instantiatedProducts.Add(product.productName, (instance, newMaterial));
        }
        else
        {
            Debug.LogWarning($"Un oggetto con il nome {product.productName} esiste già nella scena!");
        }

        
        
    }


     public static void GenerateBlock(Productinfo product, GameObject parent, int rows, int columns, int levels, Vector3 spacingOverride)
    {

        if (product.prefabs == null )
        {
            Debug.LogError("Prefab non assegnati!");
            return;
        }
        GameObject instance1  = Instantiate(product.prefabs,product._positions,product.prefabs.transform.rotation,parent.transform);
        LoadMesh(instance1,product.numberOfProduct);   

        // Ottieni le dimensioni della prima istanza
        Renderer renderer = instance1.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("L'instance1 non ha un Renderer! Assicurati che il prefab abbia un componente Renderer per calcolare le dimensioni.");
            return;
        }

        // Dimensioni calcolate dal prefab
        Vector3 prefabSize = renderer.bounds.size;
        //Vector3 prefabSize=instance1.GetComponent<MeshFilter>().mesh.bounds.size;
        // Calcola la spaziatura, usando un override opzionale
        float spacingX = spacingOverride.x > 0 ? spacingOverride.x : prefabSize.x;
        float spacingY = spacingOverride.y > 0 ? spacingOverride.y : prefabSize.y;
        float spacingZ = spacingOverride.z > 0 ? spacingOverride.z : prefabSize.z;
        Destroy(instance1);
        
        // Genera il blocco
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                for (int z = 0; z < levels; z++)
                {
                    // Calcola la posizione per ciascun prodotto
                    Vector3 newPosition = product._positions 
                        + new Vector3(x * spacingX, y * spacingY, z * spacingZ);

                    // Istanzia il prodotto
                    GameObject instance = Instantiate(product.prefabs, newPosition, product.prefabs.transform.rotation, parent.transform);

                    // Imposta il nome unico (riga, colonna, livello)
                    string uniqueName = $"{product.productName}_R{y}_C{x}_L{z}";
                    instance.name = uniqueName;
                    Material newMaterial=null;
                    LoadMesh(instance,product.numberOfProduct);
/*
                    // Assegna il materiale
                    MeshRenderer instanceRenderer = instance.GetComponent<MeshRenderer>();
                    
                    if (instanceRenderer != null)
                    {
                        newMaterial = new Material(shader);
                        instanceRenderer.material = newMaterial;
                    }
                    else
                    {
                        Debug.LogWarning($"Il prefab {product.productName} non ha un MeshRenderer!");
                    }
*/
                    // Aggiungi l'oggetto al dizionario
                    if (!instantiatedProducts.ContainsKey(uniqueName))
                    {
                        instantiatedProducts.Add(uniqueName, (instance, newMaterial));
                    }
                    else
                    {
                        Debug.LogWarning($"Oggetto con nome {uniqueName} già esistente!");
                    }
                }
            }
        }

        Debug.Log($"Blocco di prodotti generato: {rows}x{columns}x{levels}");
    }
    public static void LoadMesh(GameObject instance,int n){
        string nameProduct="CannedFish-";
        if (n<0 || n>3) {
            Debug.LogError("Il numero di elementi è fuori range!");
            n=0;
        }
        
        string nProduct=n.ToString();
        string meshFilePath="Assets/Models/Products/mesh/"+nameProduct+nProduct+".mesh"; 

        if (!File.Exists(meshFilePath))
        {
            Debug.LogError($"Il file {meshFilePath} non esiste!");
            return;
        }
        //ottieni il mesh render dell'oggetto instanziato
        MeshFilter meshfilter=instance.GetComponent<MeshFilter>();
        if (meshfilter == null)
        {
            Debug.LogError("Il prefab non ha un meshfilter!");
            return;
        }
        // Usa una libreria esterna per caricare la mesh
       // Mesh loadedMesh = ObjImporter.ImportFromFile(meshFilePath); // Usa una libreria come SimpleObjLoader

        Mesh loadedMesh=AssetDatabase.LoadAssetAtPath<Mesh>(meshFilePath);

        if (loadedMesh == null)
        {
            Debug.LogError("Caricamento della mesh fallito!");
            return;
        }

        // Assegna la mesh al MeshFilter
        meshfilter.mesh = loadedMesh;
        Debug.Log($"Mesh caricata con successo da {meshFilePath} e assegnata.");
    

        Mesh mesh=new Mesh();

        // Assegna la nuova mesh  al MeshRenderer del prefab
        meshfilter.mesh=loadedMesh;

        Debug.Log("Shader assegnato con successo al prefab istanziato."); 


    }


    /*void Destroy(Productinfo product){

        
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


    }*/
    public static void Destroy(Productinfo product)
    {
        // Trova l'oggetto nel dizionario
        if (instantiatedProducts.TryGetValue(product.productName, out var data))
        {
            GameObject instance = data.instance;
            Material material = data.material;

            // Rimuovi il riferimento dal dizionario
            instantiatedProducts.Remove(product.productName);

            // Rilascia il materiale associato
            if (material != null)
            {
                Destroy(material);
                Debug.Log($"Materiale associato all'oggetto {product.productName} rilasciato.");
            }

            // Distruggi il GameObject
            if (instance != null)
            {
                Destroy(instance);
                Debug.Log($"Oggetto {product.productName} distrutto.");
            }
        }
        else
        {
            Debug.LogWarning($"Oggetto con nome {product.productName} non trovato.");
        }
    }

    // Metodo per distruggere tutti gli oggetti e rilasciare le risorse
    public static void DestroyAll()
    {
        foreach (var kvp in instantiatedProducts)
        {
            GameObject instance = kvp.Value.instance;
            Material material = kvp.Value.material;

            // Distruggi il materiale
            if (material != null)
            {
                Destroy(material);
            }

            // Distruggi il GameObject
            if (instance != null)
            {
                Destroy(instance);
            }
        }

        instantiatedProducts.Clear();
        Debug.Log("Tutti gli oggetti e le risorse associate sono stati distrutti.");
    }
    
}

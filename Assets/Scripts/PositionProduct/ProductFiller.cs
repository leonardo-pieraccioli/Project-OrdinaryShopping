using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ProductInfoAutoFiller : EditorWindow
{
    private ProductInfo targetProductInfo; // Riferimento allo ScriptableObject da popolare
    private ProductManager productManager; // Riferimento al ProductManager in scena

    [MenuItem("Tools/Auto-Fill ProductInfo")]
    public static void ShowWindow()
    {
        GetWindow<ProductInfoAutoFiller>("Auto-Fill ProductInfo");
    }

    private void OnGUI()
    {
        GUILayout.Label("Auto-Fill ProductInfo", EditorStyles.boldLabel);
        targetProductInfo = (ProductInfo)EditorGUILayout.ObjectField("ProductInfo SO:", targetProductInfo, typeof(ProductInfo), false);
        productManager = (ProductManager)EditorGUILayout.ObjectField("Product Manager:", productManager, typeof(ProductManager), true);

        if (GUILayout.Button("Generate Product Entries"))
        {
            if (targetProductInfo != null && productManager != null)
            {
                GenerateProductEntries();
            }
            else
            {
                Debug.LogError("Seleziona ProductInfo e ProductManager!");
            }
        }

        if (GUILayout.Button("Update Product Entries"))
        {
            if (targetProductInfo != null && productManager != null)
            {
                UpdateProductEntries();
            }
            else
            {
                Debug.LogError("Seleziona ProductInfo e ProductManager!");
            }
        }
    }

    private void GenerateProductEntries()
    {
        List<Productinfo> newProducts = new List<Productinfo>();

        GameObject[] emptyObjects = GetEmptyGameObjectsFromManager();
        if (emptyObjects.Length == 0)
        {
            Debug.LogError("Nessun empty object trovato nel ProductManager!");
            return;
        }

        foreach (GameObject empty in emptyObjects)
        {
            Productinfo newProduct = new Productinfo
            {
                LabelPosition = empty.name,
                emptyPos = empty,
                _positions = empty.transform.position,
                _xn = 1,
                _yn = 1,
                _zn = 1,
                _offset = new Vector3(1f, 1f, 1f)
            };

            newProducts.Add(newProduct);
        }

        targetProductInfo.products = newProducts.ToArray();
        EditorUtility.SetDirty(targetProductInfo);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"ProductInfo popolato con {newProducts.Count} prodotti!");
    }

   private void UpdateProductEntries()
{
    if (targetProductInfo.products == null)
    {
        targetProductInfo.products = new Productinfo[0];
    }

    List<Productinfo> existingProducts = new List<Productinfo>(targetProductInfo.products);
    Dictionary<string, Productinfo> productLookup = new Dictionary<string, Productinfo>();
    
    // Crea una mappa per accedere ai prodotti esistenti tramite la loro etichetta
    foreach (Productinfo product in existingProducts)
    {
        if (!string.IsNullOrEmpty(product.LabelPosition))
        {
            productLookup[product.LabelPosition] = product;
        }
    }

    GameObject[] emptyObjects = GetEmptyGameObjectsFromManager();
    if (emptyObjects.Length == 0)
    {
        Debug.LogError("Nessun empty object trovato nel ProductManager!");
        return;
    }

    // Mantieni traccia degli empty trovati per evitare di rimuovere quelli ancora validi
    HashSet<string> foundLabels = new HashSet<string>();
    List<Productinfo> newProducts = new List<Productinfo>();

    // Aggiungi nuovi prodotti o aggiorna quelli esistenti
    foreach (GameObject empty in emptyObjects)
    {
        foundLabels.Add(empty.name); // Tieni traccia dei prodotti che esistono ancora

        if (productLookup.ContainsKey(empty.name))
        {
            // Prodotto esistente, aggiorna i dati se sono cambiati
            Productinfo existingProduct = productLookup[empty.name];
            if (existingProduct._positions != empty.transform.position)
            {
                Debug.Log($"Aggiornata posizione per il prodotto {existingProduct.LabelPosition}");
                existingProduct._positions = empty.transform.position;
            }
            existingProduct.emptyPos = empty;
        }
        else
        {
            // Nuovo prodotto
            Productinfo newProduct = new Productinfo
            {
                LabelPosition = empty.name,
                emptyPos = empty,
                _positions = empty.transform.position,
                _xn = 1,
                _yn = 1,
                _zn = 1,
                _offset = new Vector3(1f, 1f, 1f)
            };

            newProducts.Add(newProduct);
        }
    }

    // Rimuovi prodotti che non hanno più un empty corrispondente
    List<Productinfo> updatedProducts = new List<Productinfo>();
    foreach (Productinfo product in existingProducts)
    {
        if (foundLabels.Contains(product.LabelPosition))
        {
            updatedProducts.Add(product); // Mantieni solo i prodotti ancora presenti
        }
        else
        {
            Debug.LogWarning($"Rimosso il prodotto {product.LabelPosition} perché il relativo empty non esiste più.");
        }
    }

    // Aggiungi i nuovi prodotti trovati
    updatedProducts.AddRange(newProducts);
    targetProductInfo.products = updatedProducts.ToArray();

    EditorUtility.SetDirty(targetProductInfo);
    AssetDatabase.SaveAssets();
    AssetDatabase.Refresh();

    Debug.Log($"Aggiornato ProductInfo con {newProducts.Count} nuovi prodotti e rimosso quelli eliminati.");
}

    private GameObject[] GetEmptyGameObjectsFromManager()
    {
        List<GameObject> emptyObjects = new List<GameObject>();

        if (productManager == null)
        {
            Debug.LogError("ProductManager non assegnato!");
            return emptyObjects.ToArray();
        }

        foreach (Transform container in productManager.transform)
        {
            foreach (Transform child in container)
            {
                if (child.childCount == 0 && child.GetComponents<Component>().Length == 1) // Controlla se è un vero "empty"
                {
                    emptyObjects.Add(child.gameObject);
                }
            }
        }

        return emptyObjects.ToArray();
    }
}

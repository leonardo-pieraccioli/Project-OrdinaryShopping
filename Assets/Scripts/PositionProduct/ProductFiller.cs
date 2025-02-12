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

    /// <summary>
    /// Trova tutti gli empty figli del ProductManager.
    /// </summary>
    private GameObject[] GetEmptyGameObjectsFromManager()
    {
        List<GameObject> emptyObjects = new List<GameObject>();

        if (productManager == null)
        {
            Debug.LogError("ProductManager non assegnato!");
            return emptyObjects.ToArray();
        }

        // Cicla attraverso tutti i figli di ProductManager
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

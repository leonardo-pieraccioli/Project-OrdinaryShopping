using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(ProductInfo))]
public class ProductInfoEditor : Editor
{
    private string searchQuery = ""; // Campo per la barra di ricerca
    private string searchLabel = ""; // Campo per la ricerca per label
    private List<Productinfo> filteredList = new List<Productinfo>();
    private SerializedProperty productsProperty;

    private void OnEnable()
    {
        productsProperty = serializedObject.FindProperty("products");
    }

    public override void OnInspectorGUI()
    {
        // Ottieni il riferimento al database
        ProductInfo productDatabase = (ProductInfo)target;
        serializedObject.Update();

        // Barra di ricerca
        EditorGUILayout.LabelField("Search", EditorStyles.boldLabel);
        searchQuery = EditorGUILayout.TextField("Filter by name:", searchQuery);
        searchLabel = EditorGUILayout.TextField("Filter by label position:", searchLabel);

        // Pulsante per aggiungere un nuovo prodotto
        if (GUILayout.Button("Add Product"))
        {
            ArrayUtility.Add(ref productDatabase.products, new Productinfo());
            EditorUtility.SetDirty(productDatabase);
        }

        // Filtra la lista basandosi sulla ricerca
        if (!string.IsNullOrEmpty(searchQuery) || !string.IsNullOrEmpty(searchLabel))
        {
            filteredList = productDatabase.products
                .Where(p => (string.IsNullOrEmpty(searchQuery) || (p.productName != null && p.productName.ToLower().Contains(searchQuery.ToLower()))) &&
                            (string.IsNullOrEmpty(searchLabel) || (p.LabelPosition != null && p.LabelPosition.ToLower().Contains(searchLabel.ToLower()))))
                .ToList();
        }
        else
        {
            filteredList = productDatabase.products.ToList(); // Mostra tutti se non c'è ricerca
        }

        // Mostra la lista filtrata
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Products", EditorStyles.boldLabel);

        if (filteredList.Count == 0)
        {
            EditorGUILayout.HelpBox("No products found", MessageType.Info);
        }
        else
        {
            for (int i = 0; i < productDatabase.products.Length; i++)
            {
                EditorGUILayout.BeginVertical("box");
                productDatabase.products[i].LabelPosition = EditorGUILayout.TextField("Label Position:", productDatabase.products[i].LabelPosition);
                productDatabase.products[i].productName = EditorGUILayout.TextField("Name:", productDatabase.products[i].productName);
                productDatabase.products[i].price = EditorGUILayout.FloatField("Price:", productDatabase.products[i].price);
                productDatabase.products[i].description = EditorGUILayout.TextField("Description:", productDatabase.products[i].description);
                productDatabase.products[i].isInShoppingList = EditorGUILayout.Toggle("Is in Shopping List:", productDatabase.products[i].isInShoppingList);
                productDatabase.products[i].prefabs = (GameObject)EditorGUILayout.ObjectField("Prefab:", productDatabase.products[i].prefabs, typeof(GameObject), false);
                productDatabase.products[i].emptyPos = (GameObject)EditorGUILayout.ObjectField("Empty Pos:", productDatabase.products[i].emptyPos, typeof(GameObject), false);
                productDatabase.products[i]._dimensions = EditorGUILayout.Vector3Field("Dimensions:", productDatabase.products[i]._dimensions);
                productDatabase.products[i]._positions = EditorGUILayout.Vector3Field("Positions:", productDatabase.products[i]._positions);
                productDatabase.products[i]._xn = EditorGUILayout.IntField("X Count:", productDatabase.products[i]._xn);
                productDatabase.products[i]._yn = EditorGUILayout.IntField("Y Count:", productDatabase.products[i]._yn);
                productDatabase.products[i]._zn = EditorGUILayout.IntField("Z Count:", productDatabase.products[i]._zn);
                productDatabase.products[i]._offset = EditorGUILayout.Vector3Field("Offset:", productDatabase.products[i]._offset);
                productDatabase.products[i]._rotate = EditorGUILayout.Toggle("Rotate:", productDatabase.products[i]._rotate);
                
                if (GUILayout.Button("Remove Product"))
                {
                    ArrayUtility.RemoveAt(ref productDatabase.products, i);
                    EditorUtility.SetDirty(productDatabase);
                    break;
                }
                EditorGUILayout.EndVertical();
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}

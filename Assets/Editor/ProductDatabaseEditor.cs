using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using Cinemachine.Editor;
using Unity.VisualScripting;
using Codice.CM.Client.Gui;

[CustomEditor(typeof(ProductInfo))]
public class ProductInfoEditor : Editor
{
    private string searchQuery = "";    // Filtro per il nome del prodotto
    private string searchLabel = "";    // Filtro per la label
    private string searchPrefab = "";   // Filtro per il nome del prefab
    
    private SerializedProperty scriptableTarget;
    private SerializedProperty price;

    private void OnEnable()
    {


       // scriptableTarget=serializedObject.FindProperty("ProductInfo1");
      
    }

    public override void OnInspectorGUI()
    {
        // Ottieni il riferimento al database
        ProductInfo productDatabase = (ProductInfo)target;

        serializedObject.Update();
        SerializedProperty productsProperty = serializedObject.FindProperty("products");
        // Barra di ricerca
        EditorGUILayout.LabelField("Search", EditorStyles.boldLabel);
        searchQuery = EditorGUILayout.TextField("Filter by name:", searchQuery).ToLower();
        searchLabel = EditorGUILayout.TextField("Filter by label position:", searchLabel).ToLower();
        searchPrefab = EditorGUILayout.TextField("Filter by prefab name:", searchPrefab).ToLower();

        // Pulsante per aggiungere un nuovo prodotto
        if (GUILayout.Button("Add Product"))
        {
            // Aumenta la dimensione dell'array
        
            productsProperty.InsertArrayElementAtIndex(productsProperty.arraySize);
                    
            EditorUtility.SetDirty(productDatabase);
        }

        // Mostra la lista filtrata
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Products", EditorStyles.boldLabel);

        if(productDatabase.products.Length>0){
        var filteredList = productDatabase.products
            .Where(p => (string.IsNullOrEmpty(searchQuery) || (p.productName != null && p.productName.ToLower().Contains(searchQuery))) &&
                        (string.IsNullOrEmpty(searchLabel) || (p.LabelPosition != null && p.LabelPosition.ToLower().Contains(searchLabel))) &&
                        (string.IsNullOrEmpty(searchPrefab) || (p.prefabs != null && p.prefabs.name.ToLower().Contains(searchPrefab))))
            .ToArray();

        if (filteredList.Length == 0)
        {
            EditorGUILayout.HelpBox("No products found", MessageType.Info);
        }
        else
        {
            for (int i = 0; i < productDatabase.products.Length; i++)
            {
                if (!filteredList.Contains(productDatabase.products[i]))
                    continue;

                SerializedProperty product = productsProperty.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.PropertyField(product.FindPropertyRelative("LabelPosition"), new GUIContent("LabelPosition:"));
                EditorGUILayout.PropertyField(product.FindPropertyRelative("productName"), new GUIContent("Name:"));
                EditorGUILayout.PropertyField(product.FindPropertyRelative("prefabs"));       
                EditorGUILayout.PropertyField(product.FindPropertyRelative("price"), new GUIContent("Price:"));
                if (GUILayout.Button("Update Name and Price"))
                {
                     for (int j = 0; j < productDatabase.products.Length; j++){
                   
                    SerializedProperty product2 = productsProperty.GetArrayElementAtIndex(j);
                     if(product.FindPropertyRelative("prefabs").objectReferenceInstanceIDValue==product2.FindPropertyRelative("prefabs").objectReferenceInstanceIDValue){

                            product2.FindPropertyRelative("productName").stringValue=product.FindPropertyRelative("productName").stringValue;
                            product2.FindPropertyRelative("price").floatValue=product.FindPropertyRelative("price").floatValue;

                     }
                }
                    serializedObject.ApplyModifiedProperties();
                    break;
                }
                EditorGUILayout.PropertyField(product.FindPropertyRelative("description"), new GUIContent("Description:"));
                EditorGUILayout.PropertyField(product.FindPropertyRelative("isInShoppingList"), new GUIContent("isInShoppingList:"));
                
                EditorGUILayout.PropertyField(product.FindPropertyRelative("emptyPos"));
                EditorGUILayout.PropertyField(product.FindPropertyRelative("_xn"), new GUIContent("_xn:"));
                EditorGUILayout.PropertyField(product.FindPropertyRelative("_yn"), new GUIContent("_yn:"));
                EditorGUILayout.PropertyField(product.FindPropertyRelative("_zn"), new GUIContent("_zn:"));
                EditorGUILayout.PropertyField(product.FindPropertyRelative("_offset"), new GUIContent("_offset:"));
                EditorGUILayout.PropertyField(product.FindPropertyRelative("_rotate"), new GUIContent("_rotate:"));

                
                if (GUILayout.Button("Remove Product"))
                {
                    productsProperty.DeleteArrayElementAtIndex(i);
                    serializedObject.ApplyModifiedProperties();
                    break;
                }

                EditorGUILayout.EndVertical();
            }
        }
        }

        serializedObject.ApplyModifiedProperties();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProductInfo", menuName = "ScriptableObjects/ProductInfo", order = 1)]
public class ProductInfo : ScriptableObject
{
    public Productinfo[] products;

}

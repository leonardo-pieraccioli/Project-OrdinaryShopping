using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text;


public class ListElement
{
    public ListElement(string name, bool isTaken)
    {
        this.name = name;
        this.isTaken = isTaken;
    }
    public string name;
    public bool isTaken;
}

public class GroceriesList : MonoBehaviour
{
    private static GroceriesList _instance;
    public static GroceriesList Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<GroceriesList>();
            return _instance;
        }
    }    
    [SerializeField] TMP_Text _textComponent;
    private List<ListElement> _groceriesList;

    void Awake()
    {
        _groceriesList = new List<ListElement>();
        Debug.Assert(_textComponent != null, "Unable to find text component in GroceriesList");
    }

    public void Init(ProductInfo productList)
    {   
        _groceriesList.Clear();
        foreach (Productinfo p in productList.products)
        {
            if( p.isInShoppingList )
                _groceriesList.Add(new ListElement(p.productName, false));
        }
        StringBuilder listSB = new StringBuilder();
        foreach(ListElement e in _groceriesList)
        {
            listSB.Append("- " + e.name + "\n");
        }
        _textComponent.text = listSB.ToString();
    }

    public void CheckProductFromList(string productName)
    {
        StringBuilder listSB = new StringBuilder();
        _groceriesList.Find(e => e.name == productName).isTaken = true;
        foreach(ListElement e in _groceriesList)
        {
            if (e.isTaken)
                listSB.Append("- <s>" + e.name + "</s>\n");
            else
                listSB.Append("- " + e.name + "\n");
        }
        _textComponent.text = listSB.ToString();
    }

}

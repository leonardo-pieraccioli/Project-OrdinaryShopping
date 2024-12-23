using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text;


public class GroceriesList : MonoBehaviour
{
    
    TMP_Text _textComponent;
    private List<string> _groceriesList;

    void Awake()
    {
        _groceriesList = new List<string>();
        _textComponent = GetComponent<TMP_Text>();
        Debug.Assert(_textComponent != null, "Unable to find text component");
        getGroceriesList();
        Debug.Assert(_groceriesList.Count > 0, "Empty groceries list");
    }

    void OnEnable()
    {   
        StringBuilder listSB = new StringBuilder();
        foreach(string s in _groceriesList)
        {
            listSB.Append("- " + s + "\n");
        }
        _textComponent.text = listSB.ToString();
    }

    private void getGroceriesList()
    {
        _groceriesList.Add("Mele");
        _groceriesList.Add("Pere");
        _groceriesList.Add("Patate");
        _groceriesList.Add("Zucca");
        _groceriesList.Add("Pollo");
    }
    

}

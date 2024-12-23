using TMPro;
using UnityEngine;

public class BalanceText : MonoBehaviour
{

    TMP_Text _textComponent;
    private int balance;

    void Awake()
    {
        _textComponent = GetComponent<TMP_Text>();
        Debug.Assert(_textComponent != null, "Unable to find text component");
    }

    void OnEnable()
    {   
        balance = getCurrentBalance();
        _textComponent.text = balance.ToString() + " $";
    }

    private int getCurrentBalance()
    {
        //temporary solution 
        return Random.Range(0,1000);
    }
}

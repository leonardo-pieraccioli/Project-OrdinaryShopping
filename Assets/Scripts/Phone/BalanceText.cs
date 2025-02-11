using System.Globalization;
using TMPro;
using UnityEngine;

public class BalanceText : MonoBehaviour
{
    private static BalanceText _instance;
    public static BalanceText Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<BalanceText>();
            return _instance;
        }
    }    
    [SerializeField] TMP_Text _textComponent;
    public float balance;
    public const float MINIMUM_BALANCE_EXIT = 5;

    public void SetBalance(float newBalance)
    {
        balance = newBalance;
        _textComponent.text = balance.ToString("C", CultureInfo.GetCultureInfo("en-US"));
    }

    public void UpdateBalance (float updateAmount)
    {
        balance += updateAmount;
        _textComponent.text = balance.ToString("C", CultureInfo.GetCultureInfo("en-US"));
        if (balance <= 0)
        {
            _textComponent.color = Color.red;
        }
    }
}

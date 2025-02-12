using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeDayCollision : MonoBehaviour
{
    [SerializeField] private string helpMessage = "You haven't taken any products from the shopping list. Pick up your phone with the Spacebar and navigate with Q or E to see the shopping list.";
    private FirstPersonController player;
    void Start()
    {

        // Trova automaticamente il FirstPersonController nella scena
        player = FindObjectOfType<FirstPersonController>();

    }


    // Questa funzione viene chiamata quando il collider entra in collisione con un altro collider
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se l'altro collider appartiene all'oggetto specifico che desideri
        if ( other.CompareTag("Player") )
        {
            if (BalanceText.Instance.balance >= BalanceText.MINIMUM_BALANCE_EXIT
                && !GroceriesList.Instance.IsAtLeastOneProductChecked() )
            {
                CanvasManager.Instance.ActivateCanvas(CanvasCode.CNV_HELPBOX);
                DialogueManager.Instance.WriteHelpMessage(helpMessage);
            }
            else if (DayManager.Instance.currentDay.dayNumber < 9)
            {
                // Chiama la funzione desiderata sull'oggetto che contiene questo script
                DayManager.Instance.LoadDayData(DayManager.Instance.currentDay.dayNumber + 1);
                player.ResetToStartPosition();
            }
            else
            {
                // TODO Change to scene 2
                SceneManager.LoadScene(0);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.Instance.WriteHelpMessage(string.Empty);
            CanvasManager.Instance.DeactivateCanvas(CanvasCode.CNV_HELPBOX);
        }
    }
}

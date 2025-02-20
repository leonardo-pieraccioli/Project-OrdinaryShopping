using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeDayCollision : MonoBehaviour
{
    [SerializeField] private string helpMessage = "You haven't taken any products from the shopping list. Pick up your phone with the Spacebar and navigate with Q or E to see the shopping list. Remember to pay at the counter!";
    private FirstPersonController player;
    private bool hasPlayerPaid = false;
    void Start()
    {

        // Trova automaticamente il FirstPersonController nella scena
        player = FindObjectOfType<FirstPersonController>();

    }


    // Questa funzione viene chiamata quando il collider entra in collisione con un altro collider
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se l'altro collider appartiene all'oggetto specifico che desideri
        if (other.CompareTag("Player"))
        {
            if (DayManager.Instance.currentDay.dayNumber == 5)
            {
                if (Pills.hasGivenToKid)
                { SceneManager.LoadScene(2); }
                else
                {
                    SceneManager.LoadScene(3);

                }

            }
            else if (!hasPlayerPaid)
            {
                CanvasManager.Instance.ActivateCanvas(CanvasCode.CNV_HELPBOX);
                DialogueManager.Instance.WriteHelpMessage(helpMessage);
            }
            else if (DayManager.Instance.currentDay.dayNumber < 5)
            {
                // Chiama la funzione desiderata sull'oggetto che contiene questo script
                hasPlayerPaid = false;
                CanvasManager.Instance.DeactivateCanvas(CanvasCode.CNV_HELPBOX);
                DayManager.Instance.LoadDayData(DayManager.Instance.currentDay.dayNumber + 1);
                player.ResetToStartPosition();
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

    public void Pay()
    {
        if (GroceriesList.Instance.IsAtLeastOneProductChecked())
        {
            CanvasManager.Instance.ActivateCanvas(CanvasCode.CNV_HELPBOX);
            DialogueManager.Instance.WriteHelpMessage("Thank you sweetheart for your precious help!");
            hasPlayerPaid = true;
        }
        else
        {
            CanvasManager.Instance.ActivateCanvas(CanvasCode.CNV_HELPBOX);
            DialogueManager.Instance.WriteHelpMessage("I think you forgot to buy something…");
            StartCoroutine(DeactivateHelpBox());
        }
    }

    private System.Collections.IEnumerator DeactivateHelpBox()
    {
        yield return new WaitForSeconds(4);
        CanvasManager.Instance.DeactivateCanvas(CanvasCode.CNV_HELPBOX);
    }

}

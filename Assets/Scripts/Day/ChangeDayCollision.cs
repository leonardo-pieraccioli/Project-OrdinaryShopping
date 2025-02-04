using StarterAssets;
using UnityEngine;

public class ChangeDayCollision : MonoBehaviour
{
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
        if (other.CompareTag("Player"))
        {

            if (DayManager.Instance.currentDay.dayNumber < 2)
            {
                // Chiama la funzione desiderata sull'oggetto che contiene questo script
                DayManager.Instance.LoadDayData(DayManager.Instance.currentDay.dayNumber + 1);
                player.ResetToStartPosition();
            }
            else
            {

                //Finish

            }
        }
    }
}

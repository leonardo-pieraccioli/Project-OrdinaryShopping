using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Light sceneLight; // Riferimento alla luce della scena
    [SerializeField] private LightDayInfo[] dayLightSettings; // Array di configurazioni per ogni giorno

    private int currentDayIndex = 0; // Indice del giorno attuale

    private Coroutine flickeringCoroutine;

    void Start()
    {
        ApplyLightSettings(currentDayIndex);
    }

    /// <summary>
    /// Cambia le impostazioni della luce in base all'indice del giorno.
    /// </summary>
    /// <param name="dayIndex">Indice del giorno</param>
    public void ApplyLightSettings(int dayIndex)
    {
        if (dayIndex < 0 || dayIndex >= dayLightSettings.Length)
        {
            Debug.LogWarning("Indice del giorno non valido.");
            return;
        }

        currentDayIndex = dayIndex;
        LightDayInfo settings = dayLightSettings[dayIndex];

        // Imposta il colore e l'intensità della luce
        if (sceneLight != null)
        {
            sceneLight.color = settings.lightColor;
            sceneLight.intensity = settings.intensity;
        }

        // Gestisce l'effetto di flickering
        if (settings.flickeringSettings != null && settings.flickeringSettings.enableFlickering)
        {
            if (flickeringCoroutine != null)
                StopCoroutine(flickeringCoroutine);

            flickeringCoroutine = StartCoroutine(FlickerLight(settings.flickeringSettings));
        }
        else if (flickeringCoroutine != null)
        {
            StopCoroutine(flickeringCoroutine);
            flickeringCoroutine = null;
        }
    }

    /// <summary>
    /// Coroutine per l'effetto flickering.
    /// </summary>
    /// <param name="settings">Configurazioni del flickering</param>
    /// <returns></returns>
    private IEnumerator FlickerLight(LightDayInfo.LightFlickeringSettings settings)
    {
        while (true)
        {
            // Riduce l'intensità a 0 per simulare il flicker
            sceneLight.enabled = false;
            yield return new WaitForSeconds(settings.flickeringDuration);

            // Ripristina l'intensità
            sceneLight.enabled = true;
            yield return new WaitForSeconds(settings.flickeringInterval);
        }
    }

    /// <summary>
    /// Avanza al giorno successivo.
    /// </summary>
    public void NextDay()
    {
        int nextDayIndex = (currentDayIndex + 1) % dayLightSettings.Length;
        ApplyLightSettings(nextDayIndex);
    }
}

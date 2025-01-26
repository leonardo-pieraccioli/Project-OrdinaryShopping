/*using System.Collections;
using UnityEngine;


public class BombManager : MonoBehaviour
{
    [SerializeField] private BombDayInfo[] dayBombSettings; // Configurazioni giorno per giorno
    [SerializeField] private LightManager lightManager; // Riferimento al LightManager

    private int currentDayIndex = 0; // Indice del giorno attuale

    void Start()
    {
        ApplyBombSettings(currentDayIndex);
        
    }

    /// <summary>
    /// Applica le configurazioni delle bombe per il giorno attuale.
    /// </summary>
    /// <param name="dayIndex">Indice del giorno</param>
    public void ApplyBombSettings(int dayIndex)
    {
        
        if (dayIndex < 0 || dayIndex > dayBombSettings.Length)
        {
            Debug.LogWarning("Indice del giorno non valido AudioSourceBomb.");
             Debug.LogWarning(dayBombSettings.Length + " lunghezza");
            return;
        }

        BombDayInfo settings = dayBombSettings[dayIndex];

        foreach (var bombData in settings.bombsData)
        {
            if (bombData.bombAudioSource != null && bombData.explosionNumber > 0)
            {
                StartCoroutine(HandleBombExplosions(bombData));
            }
            else
            {
                Debug.LogWarning("Dati della bomba non validi.");
                Debug.LogWarning("Audio " + bombData.bombAudioSource);
                Debug.LogWarning("Explosions " + bombData.explosionNumber);
            }
        }
    }

    /// <summary>
    /// Gestisce le esplosioni di una singola bomba.
    /// </summary>
    /// <param name="bombData">Dati della bomba</param>
    private IEnumerator HandleBombExplosions(BombDayInfo.BombExplosionData bombData)
{
    if (bombData.bombAudioSource == null)
    {
        Debug.LogWarning("Nessun GameObject assegnato per la bomba.");
        yield break;
    }

    // Ottieni il componente AudioSource dal GameObject
    AudioSource bombSource = bombData.bombAudioSource.GetComponent<AudioSource>();
    if (bombSource == null)
    {
        Debug.LogWarning($"Nessun AudioSource trovato su {bombData.bombAudioSource.name}");
        yield break;
    }

    for (int i = 0; i < bombData.explosionNumber; i++)
    {
        // Riproduce il suono dell'esplosione
        bombSource.Play();

        // Attiva il flickering delle luci
        lightManager.TriggerLightBehavior();

        // Aspetta la durata del suono prima di continuare
        yield return new WaitForSeconds(bombSource.clip.length + 0.5f); // +0.5s per sicurezza
    }

    // Dopo l'ultima esplosione, ferma il flickering
    lightManager.StopLightBehavior();
}


    /// <summary>
    /// Avanza al giorno successivo.
    /// </summary>
    public void NextDay()
    {
        currentDayIndex = (currentDayIndex + 1) % dayBombSettings.Length;
        ApplyBombSettings(currentDayIndex);
    }
}
*/


using System.Collections;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    [SerializeField] private BombDayInfo[] dayBombSettings; // Configurazioni giorno per giorno
    [SerializeField] private AudioManager audioManager; // Riferimento all'AudioManager
    [SerializeField] private LightManager lightManager; // Riferimento al LightManager

    private int currentDayIndex = 0; // Indice del giorno attuale

    void Start()
    {
        ApplyBombSettings(currentDayIndex);
    }

    /// <summary>
    /// Applica le configurazioni delle bombe per il giorno attuale.
    /// </summary>
    /// <param name="dayIndex">Indice del giorno</param>
    public void ApplyBombSettings(int dayIndex)
    {
        if (dayIndex < 0 || dayIndex >= dayBombSettings.Length)
        {
            Debug.LogWarning("Indice del giorno non valido.");
            return;
        }

        BombDayInfo settings = dayBombSettings[dayIndex];

        for (int i = 0; i < settings.bombsData.Length; i++)
        {
            var bombData = settings.bombsData[i];
            var bombAudioSource = audioManager.GetBombAudioSource(i);

            if (bombAudioSource != null && bombData.explosionNumber > 0)
            {
                StartCoroutine(HandleBombExplosions(bombAudioSource, bombData.explosionNumber));
            }
            else
            {
                Debug.LogWarning("Dati della bomba non validi o AudioSource mancante.");
            }
        }
    }

    /// <summary>
    /// Gestisce le esplosioni di una singola bomba.
    /// </summary>
    /// <param name="bombAudioSource">AudioSource della bomba</param>
    /// <param name="explosionNumber">Numero di esplosioni</param>
    private IEnumerator HandleBombExplosions(AudioSource bombAudioSource, int explosionNumber)
    {
        for (int i = 0; i < explosionNumber; i++)
        {
            // Riproduce il suono dell'esplosione
            bombAudioSource.Play();

            // Attiva il flickering delle luci
            lightManager.TriggerLightBehavior();

            // Aspetta la durata del suono prima di continuare
            yield return new WaitForSeconds(bombAudioSource.clip.length + 0.5f); // +0.5s per sicurezza
        }

        // Dopo l'ultima esplosione, ferma il flickering
        lightManager.StopLightBehavior();
    }

    /// <summary>
    /// Avanza al giorno successivo.
    /// </summary>
    public void NextDay()
    {
        currentDayIndex = (currentDayIndex + 1) % dayBombSettings.Length;
        ApplyBombSettings(currentDayIndex);
    }
}

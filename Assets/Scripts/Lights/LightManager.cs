using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [Header("---Lights---")]
    [SerializeField] private Light [] sceneLights; // Riferimento alla luce della scena
    

    [Header("---Music Clips---")]
    public AudioClip[] background;
    public AudioClip explosionSound; // Suono dell'esplosione
    public AudioClip flickeringSound; // Suono del flickering

    [Header("---Audio Sources---")]
    [SerializeField] public AudioSource musicSource; // Sorgente audio per la musica
    [SerializeField] public AudioSource SFXSource; // Sorgente audio per gli effetti sonori

    [Header("---Bomb Audio Sources---")]
    [SerializeField] private AudioSource [] bombAudioSources;




    private LightDayInfo dayLightSetting;

    private static LightManager _instance;
    public static LightManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LightManager>();
            }

            return _instance;
        }
    }    

    //private int currentDayIndex = 0; // Indice del giorno attuale

    
    private Coroutine flickeringCoroutine;
    private Coroutine explosionCoroutine;

    void Start()
    {
    }


    public void Init(LightDayInfo currentLightInfo)
    {
        if (currentLightInfo != null && sceneLights.Length > 0)
        {
            dayLightSetting = currentLightInfo;
            ApplyLightSettings();

            // Avvia la musica di background   
            // !!!Bisogna fare controllo su che parte del gioco siamo per decidere quale musica di background far partire!!!
            if (musicSource != null && background.Length > 0)
            {
                musicSource.clip = background[Random.Range(0, background.Length)];
                musicSource.loop = true;
                musicSource.Play();
            }

            // Avvia il ciclo delle esplosioni
            if (dayLightSetting.flickeringSettings != null && dayLightSetting.flickeringSettings.enableFlickering)
            {
                if (explosionCoroutine != null)
                    StopCoroutine(explosionCoroutine);
                
                explosionCoroutine = StartCoroutine(ExplosionCycle());
            }
        }
        else
        {
            Debug.LogError("Configurazione luce non trovata o sceneLights non impostate.");
        }
    }

    

    /// <summary>
    /// Cambia le impostazioni della luce in base all'indice del giorno.
    /// </summary>
    /// <param name="dayIndex">Indice del giorno</param>
    /*public void ApplyLightSettings()
    {
        // Imposta colore e intensità della luce

        foreach (Light light in sceneLights)
        {
            if (light != null)
            {
                light.intensity = dayLightSetting.intensity;
                light.color = dayLightSetting.lightColor;
            }
        }

        // Gestisce l'effetto di flickering
        if (dayLightSetting.flickeringSettings != null && dayLightSetting.flickeringSettings.enableFlickering)
        {
            if (flickeringCoroutine != null)
                StopCoroutine(flickeringCoroutine);

            flickeringCoroutine = StartCoroutine(FlickerLight(dayLightSetting.flickeringSettings));
        }
        else if (flickeringCoroutine != null)
        {
            StopCoroutine(flickeringCoroutine);
            flickeringCoroutine = null;
        }
    }*/



    private void ApplyLightSettings()
    {
        foreach (Light light in sceneLights)
        {
            if (light != null)
            {
                light.intensity = dayLightSetting.intensity;
                light.color = dayLightSetting.lightColor;
            }
        }
    }

    /// <summary>
    /// Coroutine per l'effetto flickering.
    /// </summary>
    /// <param name="settings">Configurazioni del flickering</param>
    /// <returns></returns>
    /*private IEnumerator FlickerLight(LightDayInfo.LightFlickeringSettings settings)
    {
        while (true)
        {
            foreach (Light light in sceneLights)
            {
                // Riduce l'intensità a 0 per simulare il flicker
                light.enabled = false;
                yield return new WaitForSeconds(settings.flickeringDuration);

                 // Ripristina l'intensità
                light.enabled = true;
                yield return new WaitForSeconds(settings.flickeringInterval);
            }
            
        }
    }*/

     private IEnumerator ExplosionCycle()
    {
        int explosionsLeft = dayLightSetting.flickeringSettings.numberOfExplosions;
        
        while (explosionsLeft > 0)
        {
            // Tempo casuale tra un'esplosione e l'altra (es. tra 20 e 30 secondi)
            float waitTime = Random.Range(dayLightSetting.flickeringSettings.minExplosionInterval, 
                                          dayLightSetting.flickeringSettings.maxExplosionInterval);
            yield return new WaitForSeconds(waitTime);

            // Simula l'esplosione
            TriggerExplosion();

            explosionsLeft--;
        }

        Debug.Log("Tutte le esplosioni completate.");
    }

    

    /// <summary>
    /// Attiva il flickering della luce quando avviene un'esplosione.
    /// </summary>
    private void TriggerExplosion()
    {
        Debug.Log("Esplosione avvenuta! Attivando flickering...");

        // Riproduce il suono dell'esplosione
        if (bombAudioSources != null && explosionSound != null)
        {
            bombAudioSources[Random.Range(0, bombAudioSources.Length)].PlayOneShot(explosionSound);
        }

        Invoke(nameof(TriggerFlicker), 0.5f); // Modifica "0.5f" con il tempo di ritardo desiderato

       // TriggerFlicker();
    }

    /// <summary>
    /// Attiva il flickering della luce.
    /// </summary>
    public void TriggerFlicker()
    {
        if (dayLightSetting.flickeringSettings != null && dayLightSetting.flickeringSettings.enableFlickering)
        {
            if (flickeringCoroutine != null)
                StopCoroutine(flickeringCoroutine);

            flickeringCoroutine = StartCoroutine(FlickerLight(dayLightSetting.flickeringSettings));
        }
    }

    /// <summary>
    /// Coroutine per simulare il flickering della luce.
    /// </summary>
    private IEnumerator FlickerLight(LightDayInfo.LightFlickeringSettings settings)
    {
        float elapsedTime = 0f; // Tempo trascorso dall'inizio del flickering
        float totalDuration = settings.flickeringDuration;


        // Suono del flickering
        if (SFXSource != null && flickeringSound != null)
        {
            SFXSource.PlayOneShot(flickeringSound);
        }


        while (elapsedTime < totalDuration)
        {
            foreach (Light light in sceneLights)
            {
                if (light != null)
                {
                    light.intensity = Random.Range(0f, settings.maxFlickeringIntensity);
                    light.enabled = !light.enabled; // Alterna lo stato acceso/spento
                }
            }

            yield return new WaitForSeconds(Random.Range(settings.flickeringInterval * 0.5f, settings.flickeringInterval * 1.5f));
            elapsedTime += settings.flickeringInterval;
        }

        ResetLights();
    }

    /// <summary>
    /// Ripristina le luci allo stato normale dopo il flickering.
    /// </summary>
    private void ResetLights()
    {
        foreach (Light light in sceneLights)
        {
            if (light != null)
            {
                light.intensity = dayLightSetting.intensity;
                light.enabled = true;
            }
        }

        flickeringCoroutine = null;
    }
}


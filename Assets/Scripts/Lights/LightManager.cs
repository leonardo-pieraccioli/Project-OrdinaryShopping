/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Light[] sceneLights; // Array di luci da gestire
    [SerializeField] private LightDayInfo[] dayLightSettings; // Array di configurazioni per ogni giorno

    private int currentDayIndex = 0; // Indice del giorno attuale che devo richiedere al manager dei giorni

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

        // Imposta il colore e l'intensità per tutte le luci
        foreach (Light light in sceneLights)
        {
            if (light != null)
            {
                light.intensity = settings.intensity;
            }
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
            foreach (Light light in sceneLights)
            {
                if (light != null)
                    light.enabled = false;
            }
            yield return new WaitForSeconds(settings.flickeringDuration);

            // Ripristina l'intensità
            foreach (Light light in sceneLights)
            {
                if (light != null)
                    light.enabled = true;
            }
            yield return new WaitForSeconds(settings.flickeringInterval);
        }
    }

    /// Avanza al giorno successivo.
    public void NextDay()
    {
        int nextDayIndex = (currentDayIndex + 1) % dayLightSettings.Length;
        ApplyLightSettings(nextDayIndex);
    }
}
*/

/// codice con comportamento randomico del flickering per le diverse luci 
/// 

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Light[] sceneLights; // Array di luci da gestire
    [SerializeField] private LightDayInfo[] dayLightSettings; // Array di configurazioni per ogni giorno

    private int currentDayIndex = 0; // Indice del giorno attuale
    private List<Coroutine> flickeringCoroutines = new List<Coroutine>();

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

        // Interrompe tutte le coroutine di flickering attuali
        foreach (var coroutine in flickeringCoroutines)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }
        flickeringCoroutines.Clear();

        // Imposta il colore e l'intensità per tutte le luci
        foreach (Light light in sceneLights)
        {
            if (light != null)
            {
                light.intensity = settings.intensity;

                // Avvia un flickering casuale per ogni luce, se abilitato
                if (settings.flickeringSettings != null && settings.flickeringSettings.enableFlickering)
                {
                    Coroutine coroutine = StartCoroutine(FlickerLightRandomly(light, settings.flickeringSettings));
                    flickeringCoroutines.Add(coroutine);
                }
            }
        }
    }

    /// <summary>
    /// Coroutine per l'effetto flickering con comportamento casuale.
    /// </summary>
    /// <param name="light">Luce su cui applicare il flickering</param>
    /// <param name="settings">Configurazioni del flickering</param>
    /// <returns></returns>
    private IEnumerator FlickerLightRandomly(Light light, LightDayInfo.LightFlickeringSettings settings)
    {
        float randomStartDelay = Random.Range(0f, settings.flickeringInterval);
        yield return new WaitForSeconds(randomStartDelay);

        while (true)
        {
            // Disabilita la luce per una durata casuale
            light.enabled = false;
            yield return new WaitForSeconds(Random.Range(0.5f * settings.flickeringDuration, 1.5f * settings.flickeringDuration));

            // Riabilita la luce per un intervallo casuale
            light.enabled = true;
            yield return new WaitForSeconds(Random.Range(0.5f * settings.flickeringInterval, 1.5f * settings.flickeringInterval));
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

*/

///////////////////////////////////////

/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Light[] sceneLights; // Array di luci da gestire
    //[SerializeField] private LightDayInfo[] dayLightSettings; // Array di configurazioni per ogni giorno

    private List<Coroutine> flickeringCoroutines = new List<Coroutine>();
    private bool isTriggered = true; // Flag per attivare/disattivare il comportamento delle luci da collegare con bombe

    /*void Start()
    {
        // Applica le impostazioni iniziali delle luci senza avviare il flickering
        ApplyLightSettings(currentDayIndex);
    }*/

/*
    static public void Init(LightDayInfo dayLightSettings) // inizializza le luci con le impostazioni del giorno
    {
        // Imposta il colore e l'intensità per tutte le luci
        

    }

    void Update()
    {
        // Controlla se ci sono bombe e flickering attivi

    }

    /// <summary>
    /// Cambia le impostazioni della luce in base all'indice del giorno.
    /// </summary>
    /// <param name="dayIndex">Indice del giorno</param>
    public void ApplyLightSettings(LightDayInfo settings)
    {
        foreach (Light light in sceneLights)
        {
            if (light != null)
            {
                light.intensity = settings.intensity;
            }
        }

        // Avvia o ferma il flickering in base al trigger
        /*if (isTriggered)
        {
            StartFlickering(settings.flickeringSettings);
        }
        else
        {
            StopFlickering();
        }*/
    //}
/*
    /// <summary>
    /// Avvia il flickering delle luci.
    /// </summary>
    /// <param name="settings">Configurazioni del flickering</param>
    private void StartFlickering(LightDayInfo.LightFlickeringSettings settings)
    {
        StopFlickering(); // Ferma eventuali coroutine già in esecuzione

        if (settings != null && settings.enableFlickering)
        {
            foreach (Light light in sceneLights)
            {
                if (light != null)
                {
                    Coroutine coroutine = StartCoroutine(FlickerLightRandomly(light, settings));
                    flickeringCoroutines.Add(coroutine);
                }
            }
        }
    }

    /// <summary>
    /// Ferma tutte le coroutine di flickering.
    /// </summary>
    private void StopFlickering()
    {
        foreach (var coroutine in flickeringCoroutines)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }

        flickeringCoroutines.Clear();
        foreach (Light light in sceneLights)
        {
            if (light != null)
                light.enabled = true; // Assicura che tutte le luci rimangano accese
        }
    }

    /// <summary>
    /// Coroutine per l'effetto flickering con comportamento casuale.
    /// </summary>
    /// <param name="light">Luce su cui applicare il flickering</param>
    /// <param name="settings">Configurazioni del flickering</param>
    /// <returns></returns>
    private IEnumerator FlickerLightRandomly(Light light, LightDayInfo.LightFlickeringSettings settings)
    {
        float randomStartDelay = Random.Range(0f, settings.flickeringInterval);
        yield return new WaitForSeconds(randomStartDelay);

        while (true)
        {
            // Disabilita la luce per una durata casuale
            light.enabled = false;
            yield return new WaitForSeconds(Random.Range(0.5f * settings.flickeringDuration, 1.5f * settings.flickeringDuration));

            // Riabilita la luce per un intervallo casuale
            light.enabled = true;
            yield return new WaitForSeconds(Random.Range(0.5f * settings.flickeringInterval, 1.5f * settings.flickeringInterval));
        }
    }

    /// <summary>
    /// Attiva il comportamento delle luci (trigger).
    /// </summary>
    public void TriggerLightBehavior()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            LightDayInfo settings = dayLightSettings[currentDayIndex];
            StartFlickeringWithDuration(settings.flickeringSettings, 1f); // Flickering per 1 secondo
        }
    }

    private void StartFlickeringWithDuration(LightDayInfo.LightFlickeringSettings settings, float duration)
    {
        StartFlickering(settings);
        StartCoroutine(StopFlickeringAfterDuration(duration));
    }

    private IEnumerator StopFlickeringAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        StopLightBehavior();
    }

    /// <summary>
    /// Disattiva il comportamento delle luci (trigger).
    /// </summary>
    public void StopLightBehavior()
    {
        if (isTriggered)
        {
            isTriggered = false;
            StopFlickering();
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

*/

//Nuova versione



using UnityEngine;
using System.Collections;

public class LightManager : MonoBehaviour
{
    [Header("Light Settings")]
    public Light [] sceneLights;
    private float baseIntensity;
    private bool isFlickering = false;

    [Header("Audio Settings")]
    public AudioSource backgroundMusic;
    public AudioSource bombSound;

    [Header("Bomb Settings")]
    public GameObject bombEffect;
    private bool bombActive = false;

    private LightDayInfo currentDayInfo;

    public void Init(LightDayInfo dayInfo)
    {
        currentDayInfo = dayInfo;
        
        // Impostazione della luce
        baseIntensity = dayInfo.intensity;
        foreach (Light light in sceneLights)
        {
            if (light != null)
            {
                light.intensity = baseIntensity;
            }
        }
        

        // Impostazione audio
        backgroundMusic.clip = dayInfo.background;
        backgroundMusic.loop = true;
        backgroundMusic.Play();
    }

    void Update()
    {
        if (currentDayInfo == null) return;

        // Gestione del flickering dinamico
        if (currentDayInfo.flickeringSettings.enableFlickering && !isFlickering)
        {
            StartCoroutine(FlickerEffect());
        }
    }

    private IEnumerator FlickerEffect()
    {
        isFlickering = true;
        while (currentDayInfo.flickeringSettings.enableFlickering)
        {
            foreach (Light light in sceneLights)
            {
                light.intensity = Random.Range(baseIntensity * 0.8f, baseIntensity * 1.2f);
            }
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        }

        foreach (Light light in sceneLights)
        {
            light.intensity = baseIntensity;
        }
        isFlickering = false;
    }

    public void TriggerBomb()
    {
        if (!bombActive)
        {
            bombActive = true;
            bombSound.Play();
            Instantiate(bombEffect, transform.position, Quaternion.identity);
            StartCoroutine(BombFlickerEffect());
        }
    }

    private IEnumerator BombFlickerEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            foreach (Light sceneLight in sceneLights)
            {
                sceneLight.enabled = false;
                yield return new WaitForSeconds(0.1f);
                sceneLight.enabled = true;
                yield return new WaitForSeconds(0.1f);
            }
            
        }
        bombActive = false;
    }
}

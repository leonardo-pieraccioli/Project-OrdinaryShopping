using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LightManager : MonoBehaviour
{
    [Header("---Lights---")]
    [SerializeField] private Light [] sceneLights; // Riferimento alla luce della scena
    [SerializeField] private GameObject [] objLights; // Riferimento alla luce della scena

    

    [Header("---Music Clips---")]
    //public AudioClip[] background;
    public AudioClip explosionSound; // Suono dell'esplosione
    public AudioClip flickeringSound; // Suono del flickering

    [Header("---Audio Sources---")]
    [SerializeField] public AudioSource musicSource; // Sorgente audio per la musica
    [SerializeField] public AudioSource supermarketMusicSource; // Sorgente audio per la musica
    
    [SerializeField] public AudioSource SFXSource; // Sorgente audio per gli effetti sonori

    [Header("---Bomb Audio Sources---")]
    [SerializeField] private AudioSource [] bombAudioSources;


    public enum BackgroundMusicType
{
    Background = 0,
    SupermarketBack = 1,
    Menu = 2
    
}


    private LightDayInfo dayLightSetting;
    public bool isGameStarted = false; // Controlla se il gioco è iniziato

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
            /*if (musicSource != null && background.Length > 0)
            {
                musicSource.clip = background[BackgroundMusicType.Background.GetHashCode()];
                musicSource.clip= background[BackgroundMusicType.SupermarketBack.GetHashCode()];
                musicSource.loop = true;
                musicSource.Play();
            }*/


            // PlayBackgroundMusic();

            // Avvia il ciclo delle esplosioni
            if (dayLightSetting.flickeringSettings != null && dayLightSetting.flickeringSettings.enableFlickering && isGameStarted)
            {
                /*if (explosionCoroutine != null)
                    StopCoroutine(explosionCoroutine);
                
                explosionCoroutine = StartCoroutine(ExplosionCycle());*/
            }
        }
        else
        {
            Debug.LogError("Configurazione luce non trovata o sceneLights non impostate.");
        }
    }
    

    public void PlayMenuMusic()
    {
        if (musicSource != null && dayLightSetting.background.Length > 2)
        {
            musicSource.clip = dayLightSetting.background[BackgroundMusicType.Menu.GetHashCode()];
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void StartGameMusic()
    {
        if (!isGameStarted) // Controlla se il gioco è già iniziato
        {
            isGameStarted = true;

            // Ferma la musica del menu
            if (musicSource.isPlaying)
                musicSource.Stop();

            if (supermarketMusicSource.isPlaying)
                supermarketMusicSource.Stop();

            if (SFXSource.isPlaying)
                SFXSource.Stop();


            if (explosionCoroutine != null)
                StopCoroutine(explosionCoroutine);

            explosionCoroutine = StartCoroutine(ExplosionCycle());


            Debug.Log("Avvio musica del supermercato!");
            // Avvia la musica del supermercato e di background
            PlayBackgroundMusic();
        }
        else
        {
            Debug.Log("Il gioco è già iniziato, rimetto la musica del supermercato.");

            /*if (!supermarketMusicSource.isPlaying && dayLightSetting.background.Length > 1)
            {
                supermarketMusicSource.Play();
                musicSource.Play();
                Debug.Log("Musica del supermercato riavviata!");
            }*/

            if (musicSource.isPlaying)
                musicSource.Stop();

            if (supermarketMusicSource.isPlaying)
                supermarketMusicSource.Stop();

            if (SFXSource.isPlaying)
                SFXSource.Stop();


            if (explosionCoroutine != null)
                StopCoroutine(explosionCoroutine);

            explosionCoroutine = StartCoroutine(ExplosionCycle());


            Debug.Log("Avvio musica del supermercato!");
            // Avvia la musica del supermercato e di background
            PlayBackgroundMusic();

        }
    }



    public void PlayBackgroundMusic()
{
    if (musicSource != null && supermarketMusicSource != null && dayLightSetting.background.Length > 1)
    {
        // Imposta la prima musica
        musicSource.clip = dayLightSetting.background[BackgroundMusicType.Background.GetHashCode()];
        musicSource.loop = true;
        musicSource.Play();

        // Imposta la seconda musica
        supermarketMusicSource.clip = dayLightSetting.background[BackgroundMusicType.SupermarketBack.GetHashCode()];
        supermarketMusicSource.loop = true;
        supermarketMusicSource.Play();
    }
}



    public void ApplyLightSettings()
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

     private IEnumerator ExplosionCycle()
    {
        int explosionsLeft = dayLightSetting.flickeringSettings.numberOfExplosions;
        bool StopMusic = dayLightSetting.StopMusic;
        
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

    void StopAudio()
    {
        musicSource.Stop();
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
        Invoke(nameof(StopAudio), 0.7f); // Modifica "0.5f" con il tempo di ritardo desiderato


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
    /*private IEnumerator FlickerLight(LightDayInfo.LightFlickeringSettings settings)
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
        for (int i = 0; i < sceneLights.Length; i++)
        {
            Light light = sceneLights[i];

            if (light != null)
            {
                // Alterna stato della luce
                light.intensity = Random.Range(0f, settings.maxFlickeringIntensity);
                light.enabled = !light.enabled;

                // Cerca il MeshRenderer associato (che è contenuto in objLights)
                if (i < objLights.Length) // Controlliamo che esista un GameObject corrispondente
                {
                    MeshRenderer meshRenderer = objLights[i].GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        Material[] materials = meshRenderer.materials;
                        for (int j = 0; j < materials.Length; j++)
                        {
                            if (materials[j].name.Contains("Material.013")) // Trova il materiale corretto
                            {
                                if (light.enabled)
                                {
                                    //materials[j].EnableKeyword("_EMISSION");
                                    materials[j].SetColor("_EmissionColor", Color.white * 3f);
                                }
                                else
                                {
                                    //materials[j].DisableKeyword("_EMISSION");
                                    materials[j].SetColor("_EmissionColor", Color.black);
                                }
                            }
                        }
                        meshRenderer.materials = materials; // Aggiorna i materiali
                    }
                }
            }
        }

        yield return new WaitForSeconds(Random.Range(settings.flickeringInterval * 0.5f, settings.flickeringInterval * 1.5f));
        elapsedTime += settings.flickeringInterval;
    }

    ResetLights();
}*/


//con random flicker tra una luce e l'altra

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
        for (int i = 0; i < sceneLights.Length; i++)
        {
            Light light = sceneLights[i];

            if (light != null)
            {
                StartCoroutine(FlickerSingleLight(light, objLights[i], settings));
            }
        }

        yield return new WaitForSeconds(Random.Range(settings.flickeringInterval * 0.5f, settings.flickeringInterval * 1.5f));
        elapsedTime += settings.flickeringInterval;
    }

    ResetLights();
}



private IEnumerator FlickerSingleLight(Light light, GameObject objLight, LightDayInfo.LightFlickeringSettings settings)
{
    float randomDelay = Random.Range(0f, 0.2f); // Ogni luce aspetta un po' prima di cambiare stato
    yield return new WaitForSeconds(randomDelay);

    // Alterna stato della luce
    light.intensity = Random.Range(0f, settings.maxFlickeringIntensity);
    light.enabled = !light.enabled;

    // Cerca il MeshRenderer associato e cambia il materiale
    if (objLight != null)
    {
        MeshRenderer meshRenderer = objLight.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Material[] materials = meshRenderer.materials;
            for (int j = 0; j < materials.Length; j++)
            {
                if (materials[j].name.Contains("Material.013"))
                {
                    if (light.enabled)
                    {
                        materials[j].SetColor("_EmissionColor", Color.white * 3f);
                    }
                    else
                    {
                        materials[j].SetColor("_EmissionColor", Color.black);
                    }
                }
            }
            meshRenderer.materials = materials;
        }
    }
}


    
    
    /// <summary>
    /// Ripristina le luci allo stato normale dopo il flickering.
    /// </summary>
    /*private void ResetLights()
    {
        foreach (Light light in sceneLights)
        {
            if (light != null)
            {
                light.intensity = dayLightSetting.intensity;
                light.enabled = true;
            }
        }

        Debug.Log("-----------------------------Flickering completato. Luci ripristinate.");

        flickeringCoroutine = null;
    }*/





    /// <summary>
/// Ripristina le luci allo stato normale dopo il flickering, inclusi i materiali emissivi.
/// </summary>




/*public void ResetLights()
{
    Debug.Log("🔄 Resetting all lights and materials...");

    foreach (Light light in sceneLights)
    {
        if (light != null)
        {
            light.intensity = dayLightSetting.intensity; // Reset intensità
            light.color = dayLightSetting.lightColor;   // Reset colore
            light.enabled = true;                      // Assicura che siano accese
        }
    }

    for (int i = 0; i < objLights.Length; i++)
    {
        if (objLights[i] != null)
        {
            MeshRenderer meshRenderer = objLights[i].GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                Material[] materials = meshRenderer.materials;
                for (int j = 0; j < materials.Length; j++)
                {
                    if (materials[j].name.Contains("Material.013")) 
                    {
                        materials[j].SetColor("_EmissionColor", Color.white * 3f); // Reset alla luce standard
                    }
                }
                meshRenderer.materials = materials; // Aggiorna i materiali
            }
        }
    }

    flickeringCoroutine = null;
}*/
















/// <summary>
/// Ripristina le luci allo stato normale dopo il flickering, inclusi i materiali emissivi.
/// </summary>
public void ResetLights()
{
    Debug.Log("🔄 Resetting all lights and materials...");

    // Reset delle luci
    for (int i = 0; i < sceneLights.Length; i++)
    {
        Light light = sceneLights[i];
        if (light != null)
        {
            light.intensity = dayLightSetting.intensity; // Reset intensità
            light.color = dayLightSetting.lightColor;   // Reset colore
            light.enabled = true;                      // Forza l'attivazione della luce
            Debug.Log($"💡 Luce {i} ripristinata - Intensità: {light.intensity}, Stato: {light.enabled}");
        }
        else
        {
            Debug.LogError($"❌ Luce {i} è NULL! Controlla che sia assegnata correttamente.");
        }
    }

    // Reset dei materiali emissivi delle luci
    for (int i = 0; i < objLights.Length; i++)
    {
        GameObject objLight = objLights[i];
        if (objLight != null)
        {
            MeshRenderer meshRenderer = objLight.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                Material[] materials = meshRenderer.materials;
                for (int j = 0; j < materials.Length; j++)
                {
                    if (materials[j].name.Contains("Material.013")) // Verifica che sia il materiale corretto
                    {
                        materials[j].SetColor("_EmissionColor", Color.white * 3f); // Materiale torna bianco
                        materials[j].EnableKeyword("_EMISSION"); // Forza l'aggiornamento dell'emissione
                        Debug.Log($"✨ Material.013 ripristinato su {objLight.name}");
                    }
                }
                meshRenderer.materials = materials; // Aggiorna i materiali
            }
            else
            {
                Debug.LogError($"❌ MeshRenderer mancante su {objLight.name}");
            }
        }
        else
        {
            Debug.LogError($"❌ ObjLight {i} è NULL! Controlla che sia assegnato.");
        }
    }

    flickeringCoroutine = null;
}



}


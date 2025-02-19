using System.Collections;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

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
    [SerializeField] public AudioSource musicSource; // Sorgente audio per la musica 1
    [SerializeField] public AudioSource musicSource2; // Sorgente audio per la musica 2

    [SerializeField] public AudioSource supermarketAmbientSource; // Sorgente audio per la musica
    
    [SerializeField] public AudioSource SFXSource; // Sorgente audio per gli effetti sonori

    [SerializeField] public AudioSource SFXLight; // Sorgente audio per gli effetti sonori


    [Header("---Bomb Audio Sources---")]
    [SerializeField] public AudioSource [] bombAudioSources;


    public enum BackgroundMusicType
{
    Background = 0,
    Ambient = 1,
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

    private CinemachineVirtualCamera cineVC;

    void Start()
    {
        cineVC = FindObjectOfType<CinemachineVirtualCamera>();
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
    


    //non dovrebbe servire più
    public void PlayMenuMusic()
    {
        if (supermarketAmbientSource.isPlaying)
            supermarketAmbientSource.Stop();
        
        if (SFXSource.isPlaying)
            SFXSource.Stop();

        if (musicSource != null && dayLightSetting.Music.Length > 2)
        {
            musicSource.clip = dayLightSetting.Music[BackgroundMusicType.Menu.GetHashCode()];
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    //////////////////////////////////////////////////////////////////////////

    public void StopCoroutineBombs()
    {
        foreach (AudioSource audioSource in bombAudioSources)
        {
            audioSource.Stop();
        }

        if (explosionCoroutine != null)
                StopCoroutine(explosionCoroutine);
    }

    public void StartGameMusic()
    {
        if (!isGameStarted) // Controlla se il gioco è già iniziato
        {
            isGameStarted = true;

            // Ferma la musica del menu
            if (musicSource.isPlaying)
                musicSource.Stop();

            if (musicSource2.isPlaying)
                musicSource2.Stop();

            if (supermarketAmbientSource.isPlaying)
                supermarketAmbientSource.Stop();

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

            if (musicSource.isPlaying)
                musicSource.Stop();

            if (musicSource2.isPlaying) 
                musicSource2.Stop();

            if (supermarketAmbientSource.isPlaying)
                supermarketAmbientSource.Stop();

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
    if (musicSource != null && musicSource2 != null && supermarketAmbientSource != null && dayLightSetting.Music.Length > 1)
    {
        // Imposta la prima musica
        musicSource.clip = dayLightSetting.Music[BackgroundMusicType.Background.GetHashCode()];
        musicSource.loop = true;
        musicSource.Play();

        //imposta seconda music source
        musicSource2.clip = dayLightSetting.Music[BackgroundMusicType.Background.GetHashCode()];
        musicSource2.loop = true;
        musicSource2.Play();

        // Imposta la seconda musica
        supermarketAmbientSource.clip = dayLightSetting.Ambient;
        supermarketAmbientSource.loop = true;
        supermarketAmbientSource.Play();
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
        musicSource2.Stop();
        SFXSource.Stop();
        supermarketAmbientSource.Stop();
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

        if (cineVC != null)
            StartCoroutine(CameraShake());  
        DialogueManager.Instance.BombAnimations();
        Invoke(nameof(TriggerFlicker), 0.5f); // Modifica "0.5f" con il tempo di ritardo desiderato
        Invoke(nameof(StopAudio), 0.7f); // Modifica "0.5f" con il tempo di ritardo desiderato
        

       // TriggerFlicker();
    }

    private IEnumerator CameraShake()
    {
        float amplitude = 1f;
        float frequency = 0.7f;
        float duration = .5f; // Duration of the shake
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            float currentAmplitude = Mathf.Lerp(amplitude, 0.0f, elapsedTime / duration);
            float currentFrequency = Mathf.Lerp(frequency, 0.0f, elapsedTime / duration);

            cineVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = currentAmplitude;
            cineVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = currentFrequency;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the shake values are reset to 0
        cineVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.0f;
        cineVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0.0f;
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



//con random flicker tra una luce e l'altra

private IEnumerator FlickerLight(LightDayInfo.LightFlickeringSettings settings)
{
    float elapsedTime = 0f; // Tempo trascorso dall'inizio del flickering
    float totalDuration = settings.flickeringDuration;

        // Suono del flickering
        if (SFXLight != null && flickeringSound != null)
        {

            SFXLight.clip= flickeringSound;
            SFXLight.Play();
           
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
/// Ripristina le luci allo stato normale dopo il flickering, inclusi i materiali emissivi.
/// </summary>
public void ResetLights()
{
    Debug.Log("Resetting all lights and materials...");

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
                Debug.LogError($"MeshRenderer mancante su {objLight.name}");
            }
        }
        else
        {
            Debug.LogError($"ObjLight {i} è NULL! Controlla che sia assegnato.");
        }
    }

    flickeringCoroutine = null;
}



}


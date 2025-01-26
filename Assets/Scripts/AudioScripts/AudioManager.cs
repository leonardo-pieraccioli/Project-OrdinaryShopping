/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---Audio Sources---")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("---Music Clips---")]
    public AudioClip[] background;
    public AudioClip[] bombs;
    public AudioClip[] diary;
    public AudioClip[] menu;
    public AudioClip[] walking;
    // Start is called before the first frame update


    void Start()
    {
        PlayBackgroundMusic(0); // Parte con il primo brano di sottofondo
    }

    /// <summary>
    /// Riproduce la musica di sottofondo.
    /// </summary>
    /// <param name="index">Indice del brano nella lista.</param>
    public void PlayBackgroundMusic(int index)
    {
        if (index < 0 || index >= background.Length)
        {
            Debug.LogWarning("Indice del brano non valido.");
            return;
        }
        musicSource.clip = background[index];
        musicSource.loop = true;
        musicSource.Play();
    }

    /// <summary>
    /// Riproduce il suono di una bomba.
    /// </summary>
    /// <param name="index">Indice del suono nella lista.</param>
    public void PlayBombSound(int index)
    {
        if (index < 0 || index >= bombs.Length)
        {
            Debug.LogWarning("Indice del suono della bomba non valido.");
            return;
        }
        SFXSource.clip = bombs[index];
        SFXSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---Audio Sources---")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("---Bomb Audio Sources---")]
    [SerializeField] private List<AudioSource> bombAudioSources; // Elenco degli AudioSource per le bombe

    [Header("---Music Clips---")]
    public AudioClip[] background;

    void Start()
    {
        PlayBackgroundMusic(0); // Parte con il primo brano di sottofondo
    }

    /// <summary>
    /// Riproduce la musica di sottofondo.
    /// </summary>
    /// <param name="index">Indice del brano nella lista.</param>
    public void PlayBackgroundMusic(int index)
    {
        if (index < 0 || index >= background.Length)
        {
            Debug.LogWarning("Indice del brano non valido.");
            return;
        }
        musicSource.clip = background[index];
        musicSource.loop = true;
        musicSource.Play();
    }

    /// <summary>
    /// Ottiene un AudioSource per una bomba.
    /// </summary>
    /// <param name="index">Indice della bomba.</param>
    /// <returns>L'AudioSource corrispondente o null se non valido.</returns>
    public AudioSource GetBombAudioSource(int index)
    {
        if (index < 0 || index >= bombAudioSources.Count)
        {
            Debug.LogWarning("Indice dell'AudioSource della bomba non valido.");
            return null;
        }
        return bombAudioSources[index];
    }
}

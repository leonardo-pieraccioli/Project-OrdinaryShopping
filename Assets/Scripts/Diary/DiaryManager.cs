using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine.Audio;

public class DiaryManager : MonoBehaviour
{
    #region Singleton
    private static DiaryManager _instance;

    public static DiaryManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<DiaryManager>();

            return _instance;
        }
    }
    #endregion

    #region UI Elements
    [Header("Canvas Elements")]
    [SerializeField] private Canvas diaryCanvas; // Il Canvas del diario
    [SerializeField] private TextMeshProUGUI dayNameText; // Nome del giorno
    [SerializeField] private TextMeshProUGUI dayText; // Testo del diario
    [SerializeField] private Image dayImage; // Immagine del giorno
    [SerializeField] private Image fadeOverlay; // Overlay per la sfumatura a nero
    #endregion

    private FirstPersonController controller;
    private DiaryDay currentDay; // Giorno attuale
    private LightManager lightManager;

    [SerializeField] private AudioMixerGroup voiceOverMixer;
    [SerializeField] public AudioSource voiceOverSource;
    [SerializeField] public AudioSource diaryMusic;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CloseDiary();
            lightManager.StartGameMusic();
        }   
    }

    void Awake()
    {
        // Assicura il Singleton
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);


        controller = GameObject.FindObjectOfType<FirstPersonController>();
        //voiceOverSource = transform.AddComponent<AudioSource>();
        //voiceOverSource.playOnAwake = false;
        //voiceOverSource.loop = false;
        //voiceOverSource.spatialBlend = 0;
        // voiceOverSource.outputAudioMixerGroup = voiceOverMixer;

        // Assicura che il Canvas sia attivo
        if (diaryCanvas != null)
            diaryCanvas.gameObject.SetActive(false);

        // Assicura che il fade sia inizialmente trasparente
        if (fadeOverlay != null)
        {
            fadeOverlay.gameObject.SetActive(true);
            fadeOverlay.color = new Color(0, 0, 0, 0);
        }

    }

   /* void Start()
    {
        controller = GameObject.FindObjectOfType<FirstPersonController>();

        // Assicura che il Canvas sia attivo
        if (diaryCanvas != null)
            diaryCanvas.gameObject.SetActive(false);

        // Assicura che il fade sia inizialmente trasparente
        if (fadeOverlay != null)
        {
            fadeOverlay.gameObject.SetActive(true);
            fadeOverlay.color = new Color(0, 0, 0, 0);
        }

         StartCoroutine(ShowDiary());
    }*/

    public void Init(DiaryDay day)
    {
        if (day == null)
        {
            Debug.LogError("Nessun giorno passato al Diario!");
            return;
        }

        Debug.Log("Init() del Diario chiamato correttamente per il giorno: " + day.dayName);

        currentDay = day;
        lightManager = LightManager.Instance;

        if(lightManager.isGameStarted==true)
          lightManager.isGameStarted=false;

        // Imposta i valori del giorno nel diario
        if (dayNameText != null) dayNameText.text = day.dayName;
        if (dayText != null) dayText.text = day.dayText;
        if (voiceOverSource != null) voiceOverSource.clip = day.voiceOver;
        if (diaryMusic != null) diaryMusic.clip = day.diaryMusicClip;

        //if (dayImage != null && day.dayImage != null)
        //    dayImage.sprite = day.dayImage;
        else
            Debug.LogWarning("Nessuna immagine impostata per il giorno.");

        // Blocca il movimento del giocatore
        if (controller != null) controller.LockMovement(true);

        foreach (AudioSource audioSource in lightManager.bombAudioSources)
            {
                audioSource.Stop();
            }

            lightManager.StopCoroutineBombs();

        Cursor.visible = true;
        //voiceOverSource.Play();
        diaryMusic.Play();
        // Mostra il diario con effetto fade-in
        StartCoroutine(ShowDiary());
    }

    private IEnumerator ShowDiary()
    {
        if (diaryCanvas != null)
            diaryCanvas.gameObject.SetActive(true);
            //lightManager.PlayMenuMusic();
            //diaryMusic.Play();

             if (lightManager.SFXSource.isPlaying)
                lightManager.SFXSource.Stop();

            foreach (AudioSource audioSource in lightManager.bombAudioSources)
            {
                audioSource.Stop();
            }

            lightManager.musicSource.Stop();
            lightManager.musicSource2.Stop();
            voiceOverSource.Play();
            Debug.Log("Mostra il diario con effetto fade-in");

        yield return StartCoroutine(FadeIn());
    }

    public void CloseDiary()
    {
        voiceOverSource.Stop();
        diaryMusic.Stop();
        StopAllCoroutines(); // Previene sovrapposizioni di fade
        StartCoroutine(HideDiary());
    }

    private IEnumerator HideDiary()
    {
        yield return StartCoroutine(FadeOut());

        if (diaryCanvas != null)
            diaryCanvas.gameObject.SetActive(false);

        if(lightManager.isGameStarted==true)
          lightManager.isGameStarted=false;
          
        // Sblocca il movimento del giocatore
        if (controller != null) controller.LockMovement(false);
    }

    #region Fade Effects

    private IEnumerator FadeIn()
    {
        if (fadeOverlay == null) yield break;

        fadeOverlay.gameObject.SetActive(true);
        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            fadeOverlay.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, elapsedTime / duration));
            yield return null;
        }
        fadeOverlay.gameObject.SetActive(false);
    }

    private IEnumerator FadeOut()
    {
        if (fadeOverlay == null) yield break;

        fadeOverlay.gameObject.SetActive(true);
        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            fadeOverlay.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, elapsedTime / duration));
            yield return null;
        }
    }

    #endregion
}

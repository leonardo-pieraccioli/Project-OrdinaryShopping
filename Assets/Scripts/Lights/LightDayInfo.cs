using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLightDayInfo", menuName = "ScriptableObjects/LightDayInfo", order = 1)]
public class LightDayInfo : ScriptableObject
{

    //Light Configurations

    [System.Serializable]
    public class LightFlickeringSettings
    {
        public bool enableFlickering; // Abilita/disabilita l'effetto flickering
        public float flickeringInterval; // Intervallo di tempo tra un flicker e l'altro
        public float flickeringDuration; // Durata di ogni flicker
    }


    [Header("---Light Configurations---")]

    [SerializeField] public float intensity; // Intensità della luce
    public LightFlickeringSettings flickeringSettings; // Configurazioni per il flickering

    //Bomb Settings

    [Header("---Bomb Configurations---")]
        public int explosionNumber; // Numero di esplosioni per questa bomba

    //Audio Settings

    [Header("---Audio Sources---")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("---Bomb Audio Sources---")]
    [SerializeField] private List<AudioSource> bombAudioSources; // Elenco degli AudioSource per le bombe

    [Header("---Music Clips---")]
    public AudioClip background;
}





using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLightDayInfo", menuName = "ScriptableObjects/LightDayInfo", order = 1)]
public class LightDayInfo : ScriptableObject
{
    [System.Serializable]
    public class LightFlickeringSettings
    {
        public bool enableFlickering; // Abilita/disabilita l'effetto flickering
        public float flickeringInterval; // Intervallo di tempo tra un flicker e l'altro (molto basso=veloce, molto basso=veloce)
        public float flickeringDuration; // Durata di ogni flicker
        public float maxFlickeringIntensity;

         public int numberOfExplosions; // Quante esplosioni devono avvenire
        public float minExplosionInterval; // Minimo tempo tra esplosioni
        public float maxExplosionInterval; // Massimo tempo tra esplosioni
    }

    public Color lightColor; // Colore della luce
    public float intensity; // Intensità della luce
    public LightFlickeringSettings flickeringSettings; // Configurazioni per il flickering

    public bool StopMusic; // Flag per interrompere la musica alla prima bomba

    [Header("Music Clips")]
    public AudioClip[] Music;

    public AudioClip Ambient;

    
    
}





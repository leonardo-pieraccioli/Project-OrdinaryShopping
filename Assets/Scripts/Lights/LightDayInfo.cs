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
        public float flickeringInterval; // Intervallo di tempo tra un flicker e l'altro
        public float flickeringDuration; // Durata di ogni flicker
    }

    public Color lightColor; // Colore della luce
    public float intensity; // Intensità della luce
    public LightFlickeringSettings flickeringSettings; // Configurazioni per il flickering
}





/*using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBombDayInfo", menuName = "ScriptableObjects/BombDayInfo", order = 1)]
public class BombDayInfo : ScriptableObject
{

    [System.Serializable]
    public class BombExplosionData
    {
        public GameObject bombAudioSource; // Empty GameObject con AudioSource
        public int explosionNumber; // Numero di esplosioni per questa bomba
    }

    public int explosionNumber; // Numero di esplosioni per questa bomba


    public BombExplosionData[] bombsData; // Dati per tutte le bombe
}

*/

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBombDayInfo", menuName = "ScriptableObjects/BombDayInfo", order = 1)]
public class BombDayInfo : ScriptableObject
{
    [System.Serializable]
    public class BombExplosionData
    {
        public int explosionNumber; // Numero di esplosioni per questa bomba
    }

    public BombExplosionData[] bombsData; // Dati per tutte le bombe
}

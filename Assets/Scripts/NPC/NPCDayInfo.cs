using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: update when supermarket is ready
public enum NPCPosition
{
    SpawnArea1,
    SpawnArea2,
    SpawnArea3,
    SpawnArea4,
    SpawnArea5,
    SpawnArea6,
    SpawnAreaBimba
}

[CreateAssetMenu(fileName = "NewNPCDayInfo", menuName = "ScriptableObjects/NPCDayInfo", order = 1)]
public class NPCDayInfo : ScriptableObject
{
    public GameObject prefab;
    public NPCPosition position;
    public string npcName;
    public bool isTrigger;
    public string[] dialogues;
    public AudioClip[] audioClips;
    public RuntimeAnimatorController animatorController;
}

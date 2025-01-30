using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: update when supermarket is ready
public enum NPCPosition
{
    Entrance,
    Isle1,
    Isle2,
    Counter
}

[CreateAssetMenu(fileName = "NewNPCDayInfo", menuName = "ScriptableObjects/NPCDayInfo", order = 1)]
public class NPCDayInfo : ScriptableObject
{
    public GameObject prefab;
    public NPCPosition position;
    public string[] dialogues;
    public RuntimeAnimatorController animatorController;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDayInfo", menuName = "ScriptableObjects/NPCDayInfo", order = 1)]
public class NPCDayInfo : ScriptableObject
{
    public string[] dialogues;
    public RuntimeAnimatorController animatorController;
}

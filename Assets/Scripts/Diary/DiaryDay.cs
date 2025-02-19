using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDiaryDay", menuName = "ScriptableObjects/DiaryDay", order = 1)]
public class DiaryDay : ScriptableObject
{
    public string dayName; // Il nome del giorno
    
    public string dayText; // Il testo che apparirà nel Canvas

    public Sprite dayImage; // L'immagine che apparirà nel Canvas

    public AudioClip voiceOver; // Il voice over che verrà riprodotto

    public AudioClip diaryMusicClip; // La musica che verrà riprodotta
        
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioClip newMusic; // Assegna la nuova traccia dall'Inspector
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (newMusic != null)
        {
            audioSource.clip = newMusic;
            audioSource.Play();
        }
    }
}
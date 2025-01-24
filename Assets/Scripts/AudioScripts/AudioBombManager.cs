using UnityEngine;

public class AudioBombManager : MonoBehaviour
{
    [SerializeField] AudioSource bombsource;

    [SerializeField] AudioClip bombclip;


    public void Start()
    {
        bombsource.clip = bombclip;
        bombsource.Play();
    }

}

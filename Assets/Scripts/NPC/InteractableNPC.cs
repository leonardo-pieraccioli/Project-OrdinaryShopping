using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;

public class InteractableNPC : MonoBehaviour, IInteractable
{
    private TextMeshProUGUI dialogueBox;

    [Tooltip("The day info of the NPC")]
    [SerializeField] public NPCDayInfo dayInfo;
    [SerializeField] public bool isTrigger; 
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public string[] dialogue;
    public string npcName;

    [Tooltip("The animator that will be used to animate the NPC")]
    private Animator animator;
    private SphereCollider triggerCollider;
    private bool hasTalked = false;
    private void Start()
    {
        dialogue = dayInfo.dialogues;
        npcName = dayInfo.npcName;
        isTrigger = dayInfo.isTrigger;
        hasTalked = false;
        if (isTrigger)
        {
            triggerCollider = gameObject.AddComponent<SphereCollider>();
            triggerCollider.isTrigger = true;
            triggerCollider.radius = 2.5f;
        }
        audioSource = GetComponent<AudioSource>();
        audioClips = dayInfo.audioClips;
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = dayInfo.animatorController;
    }

    public void Interact(PlayerInteractor interactor)
    {
        if (isTrigger) return;
        if (DialogueManager.Instance.isDialogueHappening) return;
        if (dialogue.Length == 0) return;
        animator.SetBool("isTalking", true);
        DialogueManager.Instance.StartDialogue(this, TransitionToIdle);
    }

    private void TransitionToIdle()
    {
        animator.SetBool("isTalking", false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isTrigger && !hasTalked)
        {
            if (DialogueManager.Instance.isDialogueHappening) return;
            if (dialogue.Length == 0) return;
            animator.SetBool("isTalking", true);
            if (audioSource == null)
            {
                Debug.LogError("Audio source not found on NPC: " + npcName + " on day " + DayManager.Instance.currentDay);
            }
            else
            {
                hasTalked = true;
                audioSource.clip = audioClips[0];
                audioSource.Play();
            }
        }
    }
}

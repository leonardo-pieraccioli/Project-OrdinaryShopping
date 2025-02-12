using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableNPC : MonoBehaviour, IInteractable
{
    private TextMeshProUGUI dialogueBox;

    [Tooltip("The day info of the NPC")]
    [SerializeField] public NPCDayInfo dayInfo;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public string[] dialogue;
    public string npcName;

    [Tooltip("The animator that will be used to animate the NPC")]
    private Animator animator;

    private void Start()
    {
        dialogue = dayInfo.dialogues;
        npcName = dayInfo.npcName;
        audioSource = GetComponent<AudioSource>();
        audioClips = dayInfo.audioClips;
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = dayInfo.animatorController;
    }

    public void Interact(PlayerInteractor interactor)
    {
        if (DialogueManager.Instance.isDialogueHappening) return;
        if (dialogue.Length == 0) return;
        animator.SetBool("isTalking", true);
        DialogueManager.Instance.StartDialogue(this, TransitionToIdle);
    }

    private void TransitionToIdle()
    {
        animator.SetBool("isTalking", false);
    }
}

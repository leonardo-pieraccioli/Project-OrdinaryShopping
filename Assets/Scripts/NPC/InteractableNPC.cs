using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableNPC : MonoBehaviour, IInteractable
{
    private TextMeshProUGUI dialogueBox;

    [Tooltip("The day info of the NPC")]
    [SerializeField] NPCDayInfo dayInfo;
    private string[] dialogue;

    [Tooltip("The animator that will be used to animate the NPC")]
    private Animator animator;

    private void Start()
    {
        dialogue = dayInfo.dialogues;
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = dayInfo.animatorController;
    }

    public void Interact(PlayerInteractor interactor)
    {
        if (DialogueManager.Instance.isDialogueHappening) return;
        animator.SetBool("isTalking", true);
        DialogueManager.Instance.StartDialogue(dialogue, TransitionToIdle);
    }

    private void TransitionToIdle()
    {
        animator.SetBool("isTalking", false);
    }
}

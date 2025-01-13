using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager _instance;
    public static DialogueManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<DialogueManager>();
            }

            return _instance;
        }
    }

    private FirstPersonController playerController;
    [Tooltip("The text box where the dialogue is displayed")]
    [SerializeField] private TextMeshProUGUI dialogueBox;
    [Tooltip("The speed at which the text is displayed")]
    [SerializeField] private float textSpeed = .05f;
    private int currentDialogueIndex;
    public bool isDialogueHappening = false;
    private bool isLineRunning = false;
    private string[] dialogue;
    private Action endOfDialogueCallback;
    private Coroutine activeCoroutine;

    public void StartDialogue(string[] dialogue, Action callback)
    {
        this.dialogue = dialogue;
        this.endOfDialogueCallback = callback;

        CanvasManager.Instance.ActivateCanvas(CanvasCode.CNV_DIALOGUE);
        playerController.LockMovement(true);
        isDialogueHappening = true;
        currentDialogueIndex = 0;
        dialogueBox.text = string.Empty;
        StartCoroutine(TypeLine());
    }
    
    public void StopDialogueUI()
    {
        playerController.LockMovement(false);
        CanvasManager.Instance.ActivateCanvas(CanvasCode.CNV_HUD);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindObjectOfType<FirstPersonController>();
    }

    public void ProceedWithDialogue()
    {
        if (isLineRunning)
        {
            ReachEndOfLine();
        }
        else if (currentDialogueIndex == dialogue.Length - 1)
        {
            isDialogueHappening = false;
            dialogueBox.text = string.Empty;
            playerController.LockMovement(false);
            CanvasManager.Instance.ActivateCanvas(CanvasCode.CNV_HUD);
            endOfDialogueCallback();
        }
        else
        {
            NextLine();
        }
    }

    public void ReachEndOfLine()
    {
        StopCoroutine(activeCoroutine);
        isLineRunning = false;
        dialogueBox.text = dialogue[currentDialogueIndex];
    }

    public void NextLine()
    {
        dialogueBox.text = string.Empty;
        currentDialogueIndex++;
        activeCoroutine = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isLineRunning = true;
        foreach (char c in dialogue[currentDialogueIndex].ToCharArray())
        {
            dialogueBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isLineRunning = false;
    }
}

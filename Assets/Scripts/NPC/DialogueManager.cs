using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    #region Singleton definition
    private static DialogueManager _instance;
    public static DialogueManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<DialogueManager>();
            }

            return _instance;
        }
    }
    #endregion
    public static NPCDayInfo[] npcs;
    private FirstPersonController playerController;
    private List<GameObject> activeNPCs=new List<GameObject>();
    private int nistancenpc = 0;

    // Start is called before the first frame update
    void Start()
    {
        //activeNPCs = new List<GameObject>();
    }
    public void Init(NPCDayInfo[] currentNPCDayInfo)
    {
        if (currentNPCDayInfo != null && currentNPCDayInfo.Length > 0)
        {
            npcs = new NPCDayInfo[currentNPCDayInfo.Length];

            Array.Copy(currentNPCDayInfo, npcs, currentNPCDayInfo.Length);
        }
        else
        {
            Debug.LogError("Npc non trovato o array vuoto/null");
        }

        if (nistancenpc > 0)
        {

            DestroyNPCs();
            nistancenpc = 0;

        }

        playerController = GameObject.FindObjectOfType<FirstPersonController>();
        // MOVE TO DAY MANAGER
        foreach (NPCDayInfo npc in npcs)
        {
            SpawnNPC(npc);
            nistancenpc++;
        }
    }

    public void DestroyNPCs()
    {
        foreach (GameObject npc in activeNPCs)
        {
            Destroy(npc);
        }
    }

    #region NPC Management

    [Header("NPC Parameters")]
    [SerializeField] private Transform[] spawnPositions;
    // MOVE TO DAY MANAGER
    //[SerializeField] NPCDayInfo[] npcs;

    public void SpawnNPC(NPCDayInfo info)
    {
        GameObject npc = Instantiate(info.prefab, spawnPositions[(int)info.position]);
        npc.GetComponent<InteractableNPC>().dayInfo = info;
        activeNPCs.Add(npc);
    }



    #endregion

    #region Dialogue Management
    [Header("Dialogue parameters")]

    [Tooltip("The text box where the dialogue is displayed")]
    [SerializeField] private TextMeshProUGUI dialogueBox;

    [Tooltip("The speed at which the text is displayed")]
    [SerializeField] private float textSpeed = .05f;

    public bool isDialogueHappening = false;
    private int currentDialogueIndex;
    private bool isLineRunning = false;
    private string[] dialogue;
    private Action endOfDialogueCallback;
    private Coroutine activeCoroutine;
    public void StartDialogue(string[] dialogue, Action callback)
    {
        this.dialogue = dialogue;
        this.endOfDialogueCallback = callback;

        CanvasManager.Instance.DeactivateAllCanvasBut(CanvasCode.CNV_DIALOGUE);
        playerController.LockMovement(true);
        isDialogueHappening = true;
        currentDialogueIndex = 0;
        dialogueBox.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    public void StopDialogueUI()
    {
        playerController.LockMovement(false);
        CanvasManager.Instance.DeactivateAllCanvasBut(CanvasCode.CNV_HUD);
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
            CanvasManager.Instance.DeactivateAllCanvasBut(CanvasCode.CNV_HUD);
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

    #endregion

    #region Help Area

    [Header("Help messages parameters")]
    [SerializeField] TextMeshProUGUI helpBox;

    public void WriteHelpMessage(string message)
    {
        helpBox.text = message;
    }

    #endregion
}

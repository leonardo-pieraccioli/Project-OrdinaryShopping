using System.Collections;
using TMPro;
using UnityEngine;

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
            this.gameObject.layer = LayerMask.NameToLayer("Default");
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
        animator.SetBool("IsTalking", true);
        DialogueManager.Instance.StartDialogue(this, TransitionToIdle);
    }

    private void TransitionToIdle()
    {
        animator.SetBool("IsTalking", false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isTrigger && !hasTalked)
        {
            if (DialogueManager.Instance.isDialogueHappening) return;
            if (dialogue.Length == 0) return;
            animator.SetBool("IsTalking", true);
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
        if (!audioSource.isPlaying)
        {
            animator.SetBool("IsTalking", false);
        }
    }

    public void BombReaction()
    {
        animator.SetBool("Bomb", true);
        StartCoroutine(WaitSeconds());
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(2.5f);
        animator.SetBool("Bomb", false);
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isTrigger)
        {
            animator.SetBool("IsTalking", false);
        }
    }

    private float currentWeight = 0.0f;
    private float targetWeight;
    private float lerpSpeed = 5f;
    private float newWeight;

    void OnAnimatorIK()
    {
        if (animator.enabled && !isTrigger)
        {
            if (animator.GetBool("IsTalking"))
            {
                currentWeight = newWeight;
                targetWeight = 1f;
                newWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * lerpSpeed);
                animator.SetLookAtWeight(newWeight, 0.0f, 0.6f, 0.0f, 0.4f);
                animator.SetLookAtPosition(Camera.main.transform.position);
            }
            else 
            {
                currentWeight = newWeight;
                targetWeight = 0f;
                newWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * lerpSpeed);
                animator.SetLookAtWeight(newWeight, 0.0f, 0.5f, 0.0f, 0.4f);
                animator.SetLookAtPosition(Camera.main.transform.position);
            }
        }
    }
}

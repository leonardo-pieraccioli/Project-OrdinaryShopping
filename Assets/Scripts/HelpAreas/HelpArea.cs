using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HelpArea : MonoBehaviour
{
    [SerializeField] private string helpMessage;
    private BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        Debug.Assert(boxCollider != null, "BoxCollider absent in HelpArea object. Please fix the prefab");
    }

    private void OnTriggerEnter(Collider other)
    {
        CanvasManager.Instance.ActivateCanvas(CanvasCode.CNV_HELPBOX);
        DialogueManager.Instance.WriteHelpMessage(helpMessage);
    }

    private void OnTriggerExit(Collider other)
    {
        DialogueManager.Instance.WriteHelpMessage(string.Empty);
        CanvasManager.Instance.DeactivateCanvas(CanvasCode.CNV_HELPBOX);
    }

}

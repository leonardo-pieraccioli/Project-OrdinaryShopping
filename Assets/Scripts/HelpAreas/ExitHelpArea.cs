using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ExitHelpArea : MonoBehaviour
{
    [SerializeField] private string helpMessage;
    private BoxCollider boxCollider;
    private Material m = null;
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
        if(m == null)
        {
            GameObject exitDoor = GameObject.Find("ExitDoor");
            m = exitDoor.GetComponent<MeshRenderer>().materials[0];
        }   
        if(m.HasProperty("_Brightness"))
        {
            m.SetFloat("_Brightness", 100);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if(m.HasProperty("_Brightness"))
        {
            m.SetFloat("_Brightness", 1);
        }
        DialogueManager.Instance.WriteHelpMessage(string.Empty);
        CanvasManager.Instance.DeactivateCanvas(CanvasCode.CNV_HELPBOX);
        Destroy(gameObject);
    }

}

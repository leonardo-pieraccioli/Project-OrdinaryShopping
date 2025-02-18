using StarterAssets;
using UnityEngine;

public class Pills : MonoBehaviour, IInteractable
{
    FirstPersonController controller;
    MeshRenderer meshRenderer;
    BoxCollider boxCollider;
    public static bool hasGivenToKid = false;
    void Start()
    {   
        meshRenderer = GetComponent<MeshRenderer>();
        controller = GameObject.FindObjectOfType<FirstPersonController>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void Interact(PlayerInteractor interactor)
    {
        controller.LockMovement(true);
        CanvasManager.Instance.ActivateCanvas(CanvasCode.CNV_PILLS);
        CanvasManager.Instance.DeactivateCanvas(CanvasCode.CNV_HUD);
    }

    public void StopInteraction()
    {
        CanvasManager.Instance.ActivateCanvas(CanvasCode.CNV_HUD);
        CanvasManager.Instance.DeactivateCanvas(CanvasCode.CNV_PILLS);

        controller.LockMovement(false);
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
    }

    public static void GiveToKid()
    {
        hasGivenToKid = true;
    }
}
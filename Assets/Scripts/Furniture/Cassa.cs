using StarterAssets;
using UnityEngine;

public class Cassa : MonoBehaviour, IInteractable
{
    FirstPersonController controller;
    void Start()
    {
        controller = GameObject.FindObjectOfType<FirstPersonController>();
    }

    public void Interact(PlayerInteractor interactor)
    {
        controller.LockMovement(true);
        CanvasManager.Instance.ActivateCanvas(CanvasCode.CNV_CASSA);
        CanvasManager.Instance.DeactivateCanvas(CanvasCode.CNV_HUD);
    }

    public void StopInteraction()
    {
        CanvasManager.Instance.ActivateCanvas(CanvasCode.CNV_HUD);
        CanvasManager.Instance.DeactivateCanvas(CanvasCode.CNV_CASSA);

        controller.LockMovement(false);
    }
}
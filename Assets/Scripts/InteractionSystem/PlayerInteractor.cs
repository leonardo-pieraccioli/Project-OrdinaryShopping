using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using System;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Common variables")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GameObject playerCursorInteractor;

    [Header("Raycast variables")]
    [SerializeField] private float interactionMaxDistance = 5.0f;
    [SerializeField] private LayerMask interactionMask;

    private RaycastHit hit; // raycast hit result
    private IInteractable currentInteractable;

    void Update()
    {
        CastInteraction();    
    }

    #region Casting
    /// <summary>
    /// Casts a ray and if hit an interactable object sets 
    /// the current interactable
    /// </summary>
    private void CastInteraction()
    {
        // cast ray from camera
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, interactionMaxDistance, interactionMask))
        {
            // enter if hits an Interactable collider
            Debug.Assert(hit.collider.TryGetComponent<IInteractable>(out currentInteractable), "Forgot to add script with IInteractable!");
            if (currentInteractable == null) return;
            playerCursorInteractor.SetActive(true);
        }
        else 
        {
            // exit from interaction
            playerCursorInteractor.SetActive(false);
            currentInteractable = null;
        }
    }
    #endregion

    public void OnInteract(InputValue value)
    {
        // call interaction once on input start
        if (value.isPressed && currentInteractable != null) 
        {
            currentInteractable.Interact(this);
        }
    }
}

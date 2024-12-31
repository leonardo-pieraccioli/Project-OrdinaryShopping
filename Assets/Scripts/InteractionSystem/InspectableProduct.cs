using System;
using UnityEngine;

public class InspectableProduct : MonoBehaviour, IInteractable
{
    [Tooltip("The distance from the camera at which the object is put when it is inspected")]
    [SerializeField] static private float inspectDistance = 1f;

    private Vector3 originalPosition;
    static private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        cameraTransform = FindObjectOfType<Camera>().gameObject.transform;
    }

    public void ReachInspectPosition()
    {
        transform.position = cameraTransform.position + cameraTransform.forward * inspectDistance;
    }

    public void ReachOriginalPosition()
    {
        transform.position = originalPosition;
    }

    public void CartTheProduct()
    {
        Debug.LogError("Implement the function to insert this product in the cart");
        Destroy(gameObject);
    }

    public void Interact(PlayerInteractor interactor)
    {
        InspectManager.Instance.StartInspect(this);
        ReachInspectPosition();
    }
}

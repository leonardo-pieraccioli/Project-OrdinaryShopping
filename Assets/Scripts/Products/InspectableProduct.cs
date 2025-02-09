using System;
using UnityEngine;
using UnityEngine.Assertions;

public class InspectableProduct : MonoBehaviour, IInteractable
{
    [Tooltip("The distance from the camera at which the object is put when it is inspected")]
    [SerializeField] static private float inspectDistance = 1f;

    private ArrayInstanceProduct instanceProduct;
    private MeshRenderer meshRenderer;
    private Vector3 originalPosition;
    static private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        cameraTransform = FindObjectOfType<Camera>().gameObject.transform;
        instanceProduct = GetComponent<ArrayInstanceProduct>();
        Debug.Assert(instanceProduct != null, message: "The product must have an ArrayInstanceProduct component");
        meshRenderer = GetComponent<MeshRenderer>();
        Debug.Assert(meshRenderer != null, message: "The product must have a MeshRenderer component");
    }

    public void ReachInspectPosition()
    {
        instanceProduct.GrabObject();
        transform.position = cameraTransform.position + cameraTransform.forward * inspectDistance;
        meshRenderer.enabled = true;
    }

    public void ReachOriginalPosition()
    {
        transform.position = originalPosition;
        meshRenderer.enabled = false;
        instanceProduct.PutObject();
    }

    public void CartTheProduct()
    {
        Debug.LogError("Implement the function to insert this product in the cart");
        meshRenderer.enabled = false;
    }

    public void Interact(PlayerInteractor interactor)
    {
        InspectManager.Instance.StartInspect(this);
        ReachInspectPosition();
    }
}

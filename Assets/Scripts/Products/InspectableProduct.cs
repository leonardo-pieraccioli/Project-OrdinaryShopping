using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class InspectableProduct : MonoBehaviour, IInteractable
{
    [Tooltip("The distance from the camera at which the object is put when it is inspected")]
    [SerializeField] static private float inspectDistance = .6f;

    public ArrayInstanceProduct instanceProduct;
    private MeshRenderer meshRenderer;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    static private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        cameraTransform = FindObjectOfType<Camera>().gameObject.transform;
        instanceProduct = GetComponent<ArrayInstanceProduct>();
        Debug.Assert(instanceProduct != null, message: $"The product {gameObject.name} must have an ArrayInstanceProduct component");
        meshRenderer = GetComponent<MeshRenderer>();
        Debug.Assert(meshRenderer != null, message: "The product must have a MeshRenderer component");
    }

    public void ReachInspectPosition()
    {
        BoxCollider bc = GetComponent<BoxCollider>();
        transform.position = cameraTransform.position + cameraTransform.forward * inspectDistance - cameraTransform.up * bc.size.normalized.y / 4;
        transform.rotation = cameraTransform.rotation * Quaternion.Euler(0, 160, 0);
        
        meshRenderer.enabled = true;
    }

    public void ReachOriginalPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        meshRenderer.enabled = false;
    }

    public void CartTheProduct()
    {
        meshRenderer.enabled = false;
        GroceriesList.Instance.CheckProductFromList(instanceProduct.product.productName);
        if(BalanceText.Instance.balance >= instanceProduct.product.price)
        {
            BalanceText.Instance.UpdateBalance(-instanceProduct.product.price);
        }
    }

    public void Interact(PlayerInteractor interactor)
    {
        InspectManager.Instance.StartInspect(this);
    }

    public void setOriginalPosition(Vector3 pos)
    {
        originalPosition = pos;
    }
}

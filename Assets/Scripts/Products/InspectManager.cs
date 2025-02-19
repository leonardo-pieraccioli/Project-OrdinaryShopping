using System.Globalization;
using UnityEngine;
using StarterAssets;
using TMPro;
using System;
using Unity.VisualScripting;

public class InspectManager : MonoBehaviour
{
    private static InspectManager _instance;
    public static InspectManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<InspectManager>();
            }

            return _instance;
        }
    }
    [SerializeField] private TextMeshProUGUI productNameBox;
    [SerializeField] private TextMeshProUGUI productPriceBox;
    
    static private FirstPersonController playerController;
    private InspectableProduct inspectedProduct = null;
    public bool isInspecting = false;

    Vector3 lastMousePosition = Vector3.zero;
    private void Start()
    {
        playerController = FindObjectOfType<FirstPersonController>();
    }

    void Update()
    {
        if (isInspecting && Input.GetMouseButton(0))
        {
            Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition;
            float rotationSpeed = 1.0f;
            inspectedProduct.transform.Rotate(Vector3.up, -deltaMousePosition.x * rotationSpeed, Space.World);
            inspectedProduct.transform.Rotate(Vector3.right, deltaMousePosition.y * rotationSpeed, Space.World);
        }
        lastMousePosition = Input.mousePosition;
        
    }
    
    public void StartInspect(InspectableProduct product)
    {
        productNameBox.text = product.instanceProduct.product.productName;
        productPriceBox.text = product.instanceProduct.product.price.ToString("C", CultureInfo.GetCultureInfo("en-US"));

        isInspecting = true;
        inspectedProduct = product;
        playerController.LockMovement(true);
        CanvasManager.Instance.DeactivateAllCanvasBut(CanvasCode.CNV_INSPECT);
        inspectedProduct.ReachInspectPosition();
        inspectedProduct.gameObject.layer = 3;
        inspectedProduct.instanceProduct.GrabObject();
    }

    public void StopInspect()
    {
        isInspecting = false;
        inspectedProduct.gameObject.layer = 8;
        inspectedProduct = null;
        playerController.LockMovement(false);
        CanvasManager.Instance.DeactivateAllCanvasBut(CanvasCode.CNV_HUD);
    }

    public void LeaveProduct()
    {
        inspectedProduct.instanceProduct.PutObject();
        inspectedProduct.ReachOriginalPosition();
    }

    public void BuyProduct()
    {
        inspectedProduct.CartTheProduct();
        if(inspectedProduct.tag == "UNIQUE") //es.unicorno
        {
            inspectedProduct.setOriginalPosition(new Vector3(1000f,1000f,1000f));    
        }
        inspectedProduct.ReachOriginalPosition();
    }
}

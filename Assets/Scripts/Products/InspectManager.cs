using System.Globalization;
using UnityEngine;
using StarterAssets;
using TMPro;
using System;

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

    private void Start()
    {
        playerController = FindObjectOfType<FirstPersonController>();
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
        inspectedProduct.instanceProduct.GrabObject();
    }

    public void StopInspect()
    {
        isInspecting = false;
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

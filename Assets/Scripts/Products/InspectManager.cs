using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
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

    static private FirstPersonController playerController;
    private InspectableProduct inspectedProduct = null;

    private void Start()
    {
        playerController = FindObjectOfType<FirstPersonController>();
    }
    public void StartInspect(InspectableProduct product)
    {
        inspectedProduct = product;
        inspectedProduct.ReachInspectPosition();
        playerController.LockMovement(true);
        CanvasManager.Instance.ActivateCanvas(CanvasCode.CNV_INSPECT);
    }

    public void StopInspect()
    {
        inspectedProduct = null;
        playerController.LockMovement(false);
        CanvasManager.Instance.ActivateCanvas(CanvasCode.CNV_HUD);
    }

    public void LeaveProduct()
    {
        inspectedProduct.ReachOriginalPosition();
    }

    public void BuyProduct()
    {
        inspectedProduct.CartTheProduct();
    }
}

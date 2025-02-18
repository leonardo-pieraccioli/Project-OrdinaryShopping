using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using StarterAssets;
using Unity.VisualScripting;
using System.Linq;

public enum CanvasCode 
{
    CNV_HUD,
    CNV_INSPECT,
    CNV_DIALOGUE,
    CNV_HELPBOX,
    CNV_PHONE,
    CNV_MENU,
    CNV_CASSA
};

public class CanvasManager : MonoBehaviour
{
    List<Canvas> canvases = new List<Canvas>();

    private static CanvasManager _instance;

    public static CanvasManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<CanvasManager>();
            }

            return _instance;
        }
    }

    // temp
    // ---- 
    private FirstPersonController controller;
    private bool isMenuActive = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            canvases.Add(transform.GetChild(i).gameObject.GetComponent<Canvas>());
        }

        DeactivateAllCanvasBut(CanvasCode.CNV_HUD);

        controller = GameObject.FindAnyObjectByType<FirstPersonController>();
    }

    void Update()
    {
        if (        !canvases[(int)CanvasCode.CNV_CASSA].gameObject.activeSelf
                &&  !canvases[(int)CanvasCode.CNV_INSPECT].gameObject.activeSelf
                &&  !DiaryManager.Instance.voiceOverSource.isPlaying 
                &&  Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if ((   canvases[(int)CanvasCode.CNV_HUD].gameObject.activeSelf 
            ||  canvases[(int)CanvasCode.CNV_PHONE].gameObject.activeSelf 
            ||  canvases[(int)CanvasCode.CNV_HELPBOX].gameObject.activeSelf
            ||  canvases[(int)CanvasCode.CNV_MENU].gameObject.activeSelf
            ) && canvases[(int)CanvasCode.CNV_DIALOGUE].gameObject.activeSelf == false
            && canvases[(int)CanvasCode.CNV_INSPECT].gameObject.activeSelf == false)
        {
            if (canvases[(int)CanvasCode.CNV_MENU].gameObject.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                DeactivateCanvas(CanvasCode.CNV_MENU);
                controller.enabled = true;
                Time.timeScale = 1;
            }
            else
            {   
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                ActivateCanvas(CanvasCode.CNV_MENU);
                controller.enabled = false;
                Time.timeScale = 0;
            }
        }
    }

    public void ActivateCanvas(CanvasCode canvasCode)
    {
        CanvasCode i = 0;
        foreach(Canvas c in canvases)
        {
            if (i == canvasCode)
                c.gameObject.SetActive(true);
            i++;
        }
    }

    public void DeactivateCanvas(CanvasCode canvasCode)
    {
        if (canvasCode == CanvasCode.CNV_PHONE)
        {
            Debug.LogError("Cannote deactivate Phone Canvas. Please check your code or contact Leo ;)");
        }

        CanvasCode i = 0;
        foreach(Canvas c in canvases)
        {
            if (i == canvasCode)
                c.gameObject.SetActive(false);
            i++;
        }
    }

    public void DeactivateAllCanvasBut(CanvasCode canvasCode)
    {
        CanvasCode i = 0;
        foreach(Canvas c in canvases)
        {
            if (i == CanvasCode.CNV_PHONE)
                continue;
            else if (i == canvasCode)
                c.gameObject.SetActive(true);
            else 
                c.gameObject.SetActive(false);
            i++;
        }
    }
}

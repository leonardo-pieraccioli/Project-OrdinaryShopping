using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using StarterAssets;
using Unity.VisualScripting;

public enum CanvasCode 
{
    CNV_HUD,
    CNV_INSPECT,
    CNV_DIALOGUE,
    CNV_PHONE
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

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            canvases.Add(transform.GetChild(i).gameObject.GetComponent<Canvas>());
        }

        ActivateCanvas(CanvasCode.CNV_HUD);

        controller = GameObject.FindAnyObjectByType<FirstPersonController>();
    }

    public void ActivateCanvas(CanvasCode canvasCode)
    {
        CanvasCode i = 0;
        foreach(Canvas c in canvases)
        {
            if (i == CanvasCode.CNV_PHONE)
                continue;
            c.gameObject.SetActive(i++ == canvasCode);
        }
    }
}

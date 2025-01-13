using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum CanvasCode 
{
    CNV_HUD,
    CNV_INSPECT,
    CNV_DIALOGUE
};

public class CanvasManager : MonoBehaviour
{
    Canvas[] canvases;

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

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        canvases = GetComponentsInChildren<Canvas>();
        ActivateCanvas(CanvasCode.CNV_HUD);
    }

    public void ActivateCanvas(CanvasCode canvasCode)
    {
        CanvasCode i = 0;
        foreach(Canvas c in canvases)
        {
            c.gameObject.SetActive(i++ == canvasCode);
        }
    }
}

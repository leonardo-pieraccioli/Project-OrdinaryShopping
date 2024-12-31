using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBox : MonoBehaviour , IInteractable
{
    public void Interact(PlayerInteractor interactor)
    {
        Debug.Log("Interacted with Blue Box");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

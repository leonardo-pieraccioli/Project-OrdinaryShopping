using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBox : MonoBehaviour , IInteractable
{
    public void Interact(PlayerInteractor interactor)
    {
        StartCoroutine(Spin());
    }

    private IEnumerator Spin()
    {
        for (int i = 0; i < 90; i++)
        {
            transform.Rotate(Vector3.left, 2);
            yield return new WaitForSeconds(0.01f);
        }
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

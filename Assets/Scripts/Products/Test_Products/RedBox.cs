using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RedBox : MonoBehaviour , IInteractable
{
    public void Interact(PlayerInteractor interactor)
    {
        StartCoroutine(MoveBoxUpAndDown());
    }

    private IEnumerator MoveBoxUpAndDown()
    {
        for (int i = 0; i < 10; i++)
        {
            transform.position += Vector3.up * 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 10; i++)
        {
            transform.position += Vector3.down * 0.1f;
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

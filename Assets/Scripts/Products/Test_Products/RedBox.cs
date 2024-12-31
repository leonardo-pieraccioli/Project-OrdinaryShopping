using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RedBox : MonoBehaviour , IInteractable
{
    public void Interact(PlayerInteractor interactor)
    {
        StartCoroutine(MoveBoxUpwards());
    }

    private IEnumerator MoveBoxUpwards()
    {
        float time = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + Vector3.up * 2;
        while (time < 1)
        {
            transform.position = Vector3.Lerp(startPos, endPos, time);
            time += Time.deltaTime;
            yield return null;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeDoorInteraction : MonoBehaviour, IInteractable
{
    private bool _isOpen = false;

    public void Interact(PlayerInteractor interactor)
    {
        if(_isOpen)
        {
            StartCoroutine(CloseDoor());
        }
        else
        {
            StartCoroutine(OpenDoor());
        }
        _isOpen = !_isOpen;
    }

    private IEnumerator OpenDoor()
    {
        float angle = 0;
        float  speed = 90;
        while (angle > -95)
        {
            transform.Rotate(0, -speed * Time.deltaTime, 0);
            angle -= speed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator CloseDoor()
    {
        float angle = 0;
        float  speed = 90;
        while (angle < 95)
        {
            transform.Rotate(0, speed * Time.deltaTime, 0);
            angle += speed * Time.deltaTime;
            yield return null;
        }

    }
}

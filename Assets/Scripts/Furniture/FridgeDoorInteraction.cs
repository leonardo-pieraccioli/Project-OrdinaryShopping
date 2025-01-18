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
        for (int i = 0; i < 95; i++)
        {
            transform.Rotate(0, 1, 0);
            yield return null;
        }
    }

    private IEnumerator CloseDoor()
    {
        for (int i = 0; i < 95; i++)
        {
            transform.Rotate(0, -1, 0);
            yield return null;
        }
    }
}

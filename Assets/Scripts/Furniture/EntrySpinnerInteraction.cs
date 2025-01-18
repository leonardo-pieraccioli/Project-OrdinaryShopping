using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntrySpinnerInteraction : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInteractor interactor)
    {
        StartCoroutine(SpinDoor());
    }

    private IEnumerator SpinDoor()
    {
        for (int i = 1; i <= 120; i++)
        {
            transform.RotateAround(transform.position, transform.right, 1);
            yield return null;
        }
    }
}

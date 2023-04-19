using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : Interactable
{
    public override void Interact()
    {
        Debug.Log("Interacted");
    }
}

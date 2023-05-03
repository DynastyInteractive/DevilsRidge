using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalk : Interactable
{
    [SerializeField] DialogueSequencer _dialogueSequencer;

    public override void Interact(GameObject player)
    {
        _dialogueSequencer.StartDialogue();
    }
}

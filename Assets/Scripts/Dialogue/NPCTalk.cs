using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalk : Interactable
{
    [SerializeField] DialogueSequencer _dialogueSequencer;

    public override void Interact()
    {
        _dialogueSequencer.StartDialogue();
    }
}

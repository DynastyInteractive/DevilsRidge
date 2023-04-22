using System;
using System.Linq;
using UnityEngine;

public class DialogueSequencer : MonoBehaviour
{
    [SerializeField] DialogueSequence _dialogueSequence;
    [SerializeField] DialogueBox _dialogueBox;

    DialogueNodeData _currentNode;
    PlayerInput Input;

    public event Action OnDialogueEnded;

    void Awake()
    {
        Input = new();
        Input.Dialogue.Enable();

        _currentNode = _dialogueSequence.DialogueNodeData[0];

        _dialogueBox.gameObject.SetActive(false);
        _dialogueBox.OnChoiceButtonClicked += GoToChoice;
    }

    void Update()
    {
        if (Input.Dialogue.Next.WasPressedThisFrame() && _currentNode.Choices.Count == 0)
        {
            _dialogueBox.gameObject.SetActive(false);
            OnDialogueEnded?.Invoke();
        }
    }

    void GoToChoice(int index)
    {
        var nextNode = GetNodeFromGUID(_currentNode.Choices[index].TargetNodeGUID);
        _dialogueBox.SetDialogue(nextNode);
        _currentNode = nextNode;
    }

    DialogueNodeData GetNodeFromGUID(string targetNodeGUID)
    {
        return _dialogueSequence.DialogueNodeData.Where(node => node.NodeGUID == targetNodeGUID).ToList()[0];
    }

    public void StartDialogue()
    {
        _dialogueBox.gameObject.SetActive(true);
        _dialogueBox.SetDialogue(_currentNode);
    }
}

using System;
using System.Linq;
using UnityEngine;

public class DialogueSequencer : MonoBehaviour
{
    [SerializeField] DialogueSequence _dialogueSequence;

    DialogueNodeData _currentNode;
    PlayerInput Input;

    public event Action OnDialogueEnded;

    void Awake()
    {
        Input = new();
        Input.Dialogue.Enable();

        Reset();

        UIManager.Instance.DialogueBox.OnChoiceButtonClicked += GoToChoiceIndex;
    }


    void Update()
    {
        // if 
        if (Input.Dialogue.Next.WasPressedThisFrame() && _currentNode.Choices.Count == 0)
        {
            UIManager.Instance.HideDialogueBox();
            Reset();
            OnDialogueEnded?.Invoke();
        }
    }

    void Reset()
    {
        Debug.Log(_dialogueSequence.DialogueNodeData[0].DialogueText);
        _currentNode = _dialogueSequence.DialogueNodeData[0];
    }

    void GoToChoiceIndex(DialogueNodeData node, int index)
    {
        if (node != _currentNode) return;

        Debug.Log(gameObject.name);
        Debug.Log(index);
        var nextNode = GetNodeFromGUID(node.Choices[index].TargetNodeGUID);
        Debug.Log(nextNode.DialogueText);
        UIManager.Instance.DialogueBox.SetDialogue(nextNode);
        _currentNode = nextNode;
    }

    DialogueNodeData GetNodeFromGUID(string targetNodeGUID)
    {
        return _dialogueSequence.DialogueNodeData.Where(node => node.NodeGUID == targetNodeGUID).ToList()[0];
    }

    public void StartDialogue()
    {
        UIManager.Instance.ShowDialogueBox(_currentNode);
    }
}

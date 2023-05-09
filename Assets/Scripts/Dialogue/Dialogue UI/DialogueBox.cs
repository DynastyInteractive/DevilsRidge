using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] TMP_Text _speakerName;
    [SerializeField] TMP_Text _dialogueText;
    [SerializeField] TMP_Text _buttonPrompt;
    [SerializeField] Transform _choicesArea;
    [SerializeField] GameObject _choiceButtonPrefab;

    public event Action<DialogueNodeData, int> OnChoiceButtonClicked;

    public void SetDialogue(DialogueNodeData nodeData)
    {
        Clear();

        _speakerName.text = nodeData.SpeakerName;
        _dialogueText.text = nodeData.DialogueText;
        for (int i = 0; i < nodeData.Choices.Count; i++)
        {
            var choiceButtonObj = Instantiate(_choiceButtonPrefab, _choicesArea);
            var choiceButton = choiceButtonObj.GetComponent<DialogueChoiceButton>();
            choiceButton.Init(nodeData.Choices[i].ChoiceText, OnButtonClicked(nodeData, i));
        }
        if (nodeData.Choices.Count == 0)
            ShowButtonPrompt();
    }

    void ShowButtonPrompt()
    {
        var interactAction = new PlayerInput().Dialogue.Next;
        string key = interactAction.GetBindingDisplayString();
        string[] keys = key.Split(" | ");
        _buttonPrompt.text = $"Press [{keys[0]}]";
        _buttonPrompt.gameObject.SetActive(true);
    }

    void Clear()
    {
        _speakerName.text = string.Empty;
        _dialogueText.text = string.Empty;
        foreach (Transform choice in _choicesArea)
        {
            Destroy(choice.gameObject);
        }
        _buttonPrompt.gameObject.SetActive(false);
    }

    UnityAction OnButtonClicked(DialogueNodeData node, int i)
    {
        return () => OnChoiceButtonClicked?.Invoke(node, i);
    }
}

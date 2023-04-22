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

    public event Action<int> OnChoiceButtonClicked;

    public void SetDialogue(DialogueNodeData nodeData)
    {
        Clear();

        _speakerName.text = nodeData.SpeakerName;
        _dialogueText.text = nodeData.DialogueText;
        for (int i = 0; i < nodeData.Choices.Count; i++)
        {
            var choiceButtonObj = Instantiate(_choiceButtonPrefab, _choicesArea);
            var choiceButton = choiceButtonObj.GetComponent<DialogueChoiceButton>();
            choiceButton.Init(nodeData.Choices[i].ChoiceText, OnButtonClicked(i));
        }
        if (nodeData.Choices.Count == 0 )
            ShowButtonPrompt();
    }

    void ShowButtonPrompt()
    {
        var interactAction = new PlayerInput().Dialogue.Next;
        _buttonPrompt.text = $"Press [{interactAction.GetBindingDisplayString()}]";
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

    UnityAction OnButtonClicked(int i)
    {
        return () => OnChoiceButtonClicked?.Invoke(i);
    }
}

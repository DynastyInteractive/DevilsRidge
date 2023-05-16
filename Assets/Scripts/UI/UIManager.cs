using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] InteractPrompt _interactPrompt;
    [SerializeField] DialogueBox _dialogueBox;
    [SerializeField] GameObject _bookMenu;

    PlayerInput.UIActions Input;

    public DialogueBox DialogueBox => _dialogueBox;

    public static UIManager Instance;

    public static event Action<bool> OnUIWindowShown;

    void Awake()
    {
        Instance = this;
        Input = new PlayerInput().UI;
        Input.Enable();
    }

    void Start()
    {
        _interactPrompt.gameObject.SetActive(false);
        _dialogueBox.gameObject.SetActive(false);
        _bookMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.OpenMenu.WasPressedThisFrame()) OpenMenu();
    }

    void OpenMenu()
    {
        _bookMenu.SetActive(!_bookMenu.activeInHierarchy);
        OnUIWindowShown?.Invoke(_bookMenu.activeInHierarchy);
    }

    public void ShowInteractPrompt(Interactable interactable)
    {
        _interactPrompt.gameObject.SetActive(true);
        _interactPrompt.ShowPrompt(interactable);
    }

    public void HideInteractPrompt()
    {
        _interactPrompt.gameObject.SetActive(false);
    }

    public void ShowDialogueBox(DialogueNodeData startNode)
    {
        _dialogueBox.SetDialogue(startNode);
        _dialogueBox.gameObject.SetActive(true);
        OnUIWindowShown?.Invoke(true);
    }
    
    public void HideDialogueBox()
    {
        _dialogueBox.gameObject.SetActive(false);
        OnUIWindowShown?.Invoke(false);
    }
}

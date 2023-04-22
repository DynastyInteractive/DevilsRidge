using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UnityEngine.InputSystem.PlayerInput))]
public class InteractPrompt : MonoBehaviour
{
    [SerializeField] TMP_Text _keyText;
    [SerializeField] TMP_Text _actionText;

    UnityEngine.InputSystem.PlayerInput _playerInput;
    InputAction _interactAction;

    void Awake()
    {
        _playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();

        _interactAction = new PlayerInput().Player.Interact;
        _interactAction.Enable();
    }

    public void ShowPrompt(Interactable interactable)
    {
        _keyText.text = _interactAction.GetBindingDisplayString(group: _playerInput.currentControlScheme);
        _actionText.text = interactable.Action;
    }
}

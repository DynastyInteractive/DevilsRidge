using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] Transform _playerCamera;
    [SerializeField] LayerMask _interactionLayers;
    [SerializeField] float _maxDistance;

    PlayerInput Input;

    void Start()
    {
        Input = new PlayerInput();
        Input.Enable();
    }

    void Update()
    {
        if (FindInteractable(out Interactable interactable) && Input.Player.Interact.WasPressedThisFrame())
        {
            interactable.Interact(gameObject);
        }
    }

    // Change to constant raycast
    bool FindInteractable(out Interactable interactable)
    {
        if (!Physics.Raycast(_playerCamera.position, _playerCamera.forward, out RaycastHit hit, _maxDistance, _interactionLayers) || !hit.collider.TryGetComponent(out interactable))
        {
            interactable = null;
            UIManager.Instance.HideInteractPrompt();
            return false;
        }

        UIManager.Instance.ShowInteractPrompt(interactable);
        return true;
        
    }
}

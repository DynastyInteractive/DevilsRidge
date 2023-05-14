using UnityEngine;

public class PlayerInputEnabler : MonoBehaviour
{
    [SerializeField] FirstPersonLook _playerLookScript;
    [SerializeField] PlayerMovement _playerMovementScript;

    void Awake()
    {
        UIManager.OnUIWindowShown += SetActive;
    }

    void SetActive(bool activeState)
    {
        _playerLookScript.enabled = !activeState;
        if (activeState) _playerMovementScript.Input.Disable();
        else _playerMovementScript.Input.Enable();
    }
}

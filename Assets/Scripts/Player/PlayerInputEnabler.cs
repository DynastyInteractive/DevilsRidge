using UnityEngine;

public class PlayerInputEnabler : MonoBehaviour
{
    [SerializeField] FirstPersonLook _playerLookScript;
    [SerializeField] PlayerMovement _playerMovementScript;
    [SerializeField] PlayerCombat _playerCombatScript;
    [SerializeField] WeaponHolder _weaponHolder;

    void Awake()
    {
        UIManager.OnUIWindowShown += SetActive;
    }

    void SetActive(bool activeState)
    {
        _playerLookScript.enabled = !activeState;
        _playerCombatScript.enabled = !activeState;
        _weaponHolder.enabled = !activeState;
        if (activeState) _playerMovementScript.Input.Disable();
        else _playerMovementScript.Input.Enable();
    }
}

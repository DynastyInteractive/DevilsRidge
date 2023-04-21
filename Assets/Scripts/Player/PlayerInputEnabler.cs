using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputEnabler : MonoBehaviour
{
    [SerializeField] FirstPersonLook _playerLookScript;
    [SerializeField] PlayerMovement _playerMovementScript;

    // Start is called before the first frame update
    void Awake()
    {
        UIManager.OnUIWindowShown += (shown) => { _playerLookScript.enabled = !shown; _playerMovementScript.enabled = !shown; };
    }
}
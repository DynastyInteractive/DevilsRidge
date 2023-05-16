using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetJoinCode : MonoBehaviour
{
    [SerializeField] TMP_Text _joinCode;

    private void OnEnable()
    {
        _joinCode.text = RelayManager.JoinCode;
    }
}

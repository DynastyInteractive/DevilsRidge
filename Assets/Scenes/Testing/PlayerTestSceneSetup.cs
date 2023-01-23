using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerTestSceneSetup : MonoBehaviour
{
    [SerializeField] NetworkManager _networkManager;

    void Start()
    {
        _networkManager.StartHost();
    }
}

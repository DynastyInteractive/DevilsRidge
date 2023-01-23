using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] Button _serverButton;
    [SerializeField] Button _hostButton;
    [SerializeField] Button _clientButton;
    [SerializeField] Camera _mainCamera;

    void Awake()
    {
        _serverButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        _hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            _mainCamera.enabled = false;
        });
        _clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            _mainCamera.enabled = false;
        });
    }
}

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

    void Awake()
    {
        _serverButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        _hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        _clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}

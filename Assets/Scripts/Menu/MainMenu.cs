using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _playButton;
    [SerializeField] Button _joinButton;
    [SerializeField] TMP_InputField _joinCode;
    [SerializeField] Button _quitButton;
    [SerializeField] SceneObject _mainScene;
    
    void Awake()
    {
        _playButton.onClick.AddListener(() =>
        {
            RelayManager.isHost = true;
            SceneManager.LoadScene(_mainScene);
        });
        _joinButton.onClick.AddListener(() => 
        { 
            RelayManager.JoinCode = _joinCode.text;
            SceneManager.LoadScene(_mainScene);
        });
        _quitButton.onClick.AddListener(() =>
        {
            Debug.Log("Quit");
            Application.Quit();
        });
    }
}

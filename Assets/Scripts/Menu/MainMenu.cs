using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _playButton;
    [SerializeField] Button _quitButton;
    [SerializeField] string _gameSceneName;
    
    void Awake()
    {
        _playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(_gameSceneName);
        });
        _quitButton.onClick.AddListener(() =>
        {
            Debug.Log("Quit");
            Application.Quit();
        });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Play_game : MonoBehaviour
{
    [SerializeField] Button playButton; 
     void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Multiplayer Test");
        });
    }
}

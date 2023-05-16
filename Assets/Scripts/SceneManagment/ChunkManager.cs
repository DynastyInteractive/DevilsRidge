using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] SceneObject _sceneToManage;

    bool _isLoaded = false;

    bool _shouldLoad = false;

    void Awake()
    {
        Invoke(nameof(AllowLoading), 0.2f);
    }

    void AllowLoading()
    {
        _shouldLoad = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PlayerMovement player) || _isLoaded || !_shouldLoad) return;

        Debug.Log("Player Entered Loading Area");

        SceneManager.LoadSceneAsync(_sceneToManage, LoadSceneMode.Additive);
        _isLoaded = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out PlayerMovement player) || !_isLoaded) return;

        SceneManager.UnloadSceneAsync(_sceneToManage);
        _isLoaded = false;

    }
}

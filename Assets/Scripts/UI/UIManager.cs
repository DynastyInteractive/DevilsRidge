using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] InteractPrompt _interactPrompt;

    public static UIManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _interactPrompt.gameObject.SetActive(false);
    }

    public void ShowInteractPrompt(Interactable interactable)
    {
        _interactPrompt.gameObject.SetActive(true);
        _interactPrompt.ShowPrompt(interactable);
    }

    public void HideInteractPrompt()
    {
        _interactPrompt.gameObject.SetActive(false);
    }
}

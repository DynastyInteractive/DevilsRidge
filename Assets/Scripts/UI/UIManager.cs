using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] InteractPrompt _interactPrompt;
    [SerializeField] QuestWindow _questWindow;

    public static UIManager Instance;

    public static event Action<bool> OnUIWindowShown;

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

    public void ShowQuestWindow(Quest quest, Player player)
    {
        _questWindow.gameObject.SetActive(true);
        _questWindow.ShowQuestWindow(quest, player);
        OnUIWindowShown?.Invoke(true);
    }

    public void HideQuestWindow()
    {
        _questWindow.gameObject.SetActive(false);
        OnUIWindowShown?.Invoke(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class QuestWindow : MonoBehaviour
{
    [SerializeField] TMP_Text _title;
    [SerializeField] TMP_Text _description;
    [SerializeField] TMP_Text _goldReward;
    [SerializeField] TMP_Text _experienceReward;
    [SerializeField] Button _acceptButton;
    [SerializeField] Button _cancelButton;

    Action _onAcceptCallback;

    void Awake()
    {
        _acceptButton.onClick.AddListener(() =>
        {
            _onAcceptCallback?.Invoke();
            UIManager.Instance.HideQuestWindow();
        }) ;
        _cancelButton.onClick.AddListener(() => UIManager.Instance.HideQuestWindow());
    }

    public void ShowQuestWindow(Quest quest, Action onAcceptCallback)
    {
        _title.text = quest.Title;
        _description.text = quest.Description;
        _goldReward.text = quest.GoldReward.ToString();
        _experienceReward.text = quest.ExperienceReward.ToString();
        _onAcceptCallback = onAcceptCallback;
    }
}

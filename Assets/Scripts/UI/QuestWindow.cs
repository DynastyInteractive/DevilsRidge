using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestWindow : MonoBehaviour
{
    [SerializeField] TMP_Text _title;
    [SerializeField] TMP_Text _description;
    [SerializeField] TMP_Text _goldReward;
    [SerializeField] TMP_Text _experienceReward;
    [SerializeField] Button _acceptButton;
    [SerializeField] Button _cancelButton;

    Player _player;
    Quest _quest;

    void Awake()
    {
        _acceptButton.onClick.AddListener(AcceptQuest);
        _cancelButton.onClick.AddListener(()=> UIManager.Instance.HideQuestWindow());
    }

    public void ShowQuestWindow(Quest quest, Player player)
    {
        _title.text = quest.Title;
        _description.text = quest.Description;
        _goldReward.text = quest.GoldReward.ToString();
        _experienceReward.text = quest.ExperienceReward.ToString();
        _player = player;
        _quest = quest;
        
    }

    void AcceptQuest()
    {
        _quest.IsActive = true;
        _player.quest = _quest;
        UIManager.Instance.HideQuestWindow();
    }
}

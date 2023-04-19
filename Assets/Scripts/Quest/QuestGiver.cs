using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : Interactable 
{
    [SerializeField] Button _questButton;
    [SerializeField] GameObject _questWindow;
    [SerializeField] Button _acceptButton;

    [SerializeField] Quest _quest;

    Player _player;
    

    //public Text titleText;
    //public Text descriptionText;
    //public Text experinceText;
    //public Text goldText;

    

    void Awake()
    {
        _questButton.onClick.AddListener(OpenQuestWindow);
        _acceptButton.onClick.AddListener(AcceptQuest);
    }

    public void OpenQuestWindow()
    {
        _questWindow.SetActive(true);
        _player.GetComponent<FirstPersonLook>().enabled = false;
        //titleText.text = quest.title;
        //descriptionText.text = quest.description;
        //experinceText.text = quest.experienceReward.ToString();
        //goldText.text = quest.goldReward.ToString();
    }

    public void AcceptQuest()
    {
        _questWindow.SetActive(false);
        _quest.IsActive = true;
        _player.quest = _quest;
        _player.GetComponent<FirstPersonLook>().enabled = true;
        Debug.Log("Accepted");
    }

    public override void Interact(GameObject player)
    {
        _player = player.GetComponent<Player>();
        OpenQuestWindow();
    }
}

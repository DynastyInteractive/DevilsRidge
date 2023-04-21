using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : Interactable 
{
    [SerializeField] Quest _quest;

    Player _player;

    public void OpenQuestWindow()
    {
        UIManager.Instance.ShowQuestWindow(_quest, _player);
    }

    public void AcceptQuest()
    {
        UIManager.Instance.HideQuestWindow();
    }

    public override void Interact(GameObject player)
    {
        _player = player.GetComponent<Player>();
        OpenQuestWindow();
    }
}

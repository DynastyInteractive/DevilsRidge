using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestReceiver : MonoBehaviour {

    public Button fightButton;
    [SerializeField] int health = 5;
    int experience = 40;
    public int gold = 1000;

    [SerializeField] Quest _quest;

    bool _questIsActive=false;

    public void SetQuest(Quest quest)
    {
        _quest = quest;
        //fightButton.onClick.AddListener(GoBattle);
        _quest.OnQuestStarted += () => _questIsActive = true;
        _quest.OnQuestComplete += () => _questIsActive = false;
    }

    void GoBattle ()
    {
        health -= 1;
        experience += 2;
        gold += 5;

        if (_questIsActive)
        {
            _quest.Goal.EnemyKilled();
            if (_quest.Goal.IsReached())
            {
                experience += _quest.ExperienceReward;
                gold += _quest.GoldReward;
                _quest.CompleteQuest();
            }
        }
    }

}

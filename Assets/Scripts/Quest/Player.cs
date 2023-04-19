using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public Button fightButton;
    [SerializeField] int health = 5;
    int experience = 40;
    public int gold = 1000;

    public Quest quest;

    void Awake()
    {
        fightButton.onClick.AddListener(GoBattle);
    }

    void GoBattle ()
    {
        health -= 1;
        experience += 2;
        gold += 5;

        if (quest.IsActive)
        {
            quest.Goal.EnemyKilled();
            if (quest.Goal.IsReached())
            {
                experience += quest.ExperienceReward;
                gold += quest.GoldReward;
                quest.Complete();
            }
        }
    }

}

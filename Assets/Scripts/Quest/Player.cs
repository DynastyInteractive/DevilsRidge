using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public Button fightButton;
    public int health = 5;
    public int experience = 40;
    public int gold = 1000;

    public Quest quest;

    void Awake()
    {
        fightButton.onClick.AddListener(GoBattle);
    }

    public void GoBattle ()
    {
        health -= 1;
        experience += 2;
        gold += 5;

        if (quest._isActive)
        {
            quest.goal.EnemyKilled();
            if (quest.goal.IsReached())
            {
                experience += quest._experienceReward;
                gold += quest._goldReward;
                quest.Complete();
            }
        }
    }

}

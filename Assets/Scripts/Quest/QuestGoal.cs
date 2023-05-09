using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public GoalType goalType;

    int requiredAmount;
    [SerializeField] int currentAmount;

    public enum GoalType
    {
        Kill,
        Gathering,
    }

    public bool IsReached()
    {
        return (currentAmount >= requiredAmount);
    }

    public void EnemyKilled()
    {
        if (goalType == GoalType.Kill)
        currentAmount++;
    }

    public void ItemCollected()
    {
        if (goalType == GoalType.Gathering)
            currentAmount++;
    }

}

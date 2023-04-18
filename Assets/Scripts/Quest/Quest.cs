using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField] bool _isActive;

    [SerializeField] string _title;
    [SerializeField] string _description;
    [SerializeField] int _experienceReward;
    [SerializeField] int _goldReward;

    [SerializeField] QuestGoal _goal;

    public bool IsActive
    {
        get => _goldReward;
        set => _goldReward; = value;
    }

    public bool IsActive
    {
        get => _experienceReward;
        set => _experienceReward; = value;
    }

    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    public void Complete()
    {
        _isActive = false;
        Debug.Log(_title + " was completed");
    }
}

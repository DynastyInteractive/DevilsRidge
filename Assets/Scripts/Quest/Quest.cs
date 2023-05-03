using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField] bool _isActive;

    [SerializeField] string _title;
    [Multiline]
    [SerializeField] string _description;
    [SerializeField] int _experienceReward;
    [SerializeField] int _goldReward;

    [SerializeField] QuestGoal _goal;

    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    public string Description
    {
        get { return _description; }
        set { _description = value; }
    }

    public int GoldReward
    {
        get { return _goldReward; }
        set { _goldReward = value;} 
    }

    public int ExperienceReward
    {
        get { return _experienceReward; }
        set { _experienceReward = value; }

    }

    public bool IsActive
    {
        get { return _isActive; }
        set { _isActive = value; }
    }

    public QuestGoal Goal
    {
        get { return _goal; }
        set { _goal = value; }
    }

    public void Complete()
    {
        _isActive = false;
        Debug.Log(_title + " was completed");
    }
}

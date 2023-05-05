using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Quest",menuName ="Quests/Quest")]
public class Quest : ScriptableObject
{
  

    [SerializeField] string _title;
    [Multiline]
    [SerializeField] string _description;
    [SerializeField] int _experienceReward;
    [SerializeField] int _goldReward;

    [SerializeField] QuestGoal _goal;

    public event Action OnQuestComplete;
    public event Action OnQuestStarted;

    public string Title
    {
        get { return _title; }
        
    }

    public string Description
    {
        get { return _description; }
       
    }

    public int GoldReward
    {
        get { return _goldReward; }
        
    }

    public int ExperienceReward
    {
        get { return _experienceReward; }
       

    }

  

    public QuestGoal Goal
    {
        get { return _goal; }
       
    }

    public void CompleteQuest()
    {
        OnQuestComplete?.Invoke();
        Debug.Log(_title + " was completed");
    }

    public void StartQuest()
    {
        OnQuestStarted?.Invoke();
    }
}

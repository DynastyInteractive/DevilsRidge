using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatPanel : MonoBehaviour
{
    [SerializeField] StatDisplay[] _statDisplays;

    CharacterStat[] _stats;

    public static StatPanel Instance;

    void Awake()
    {
        Instance = this;
    }

    public void SetStats(params CharacterStat[] characterStats)
    {
        Debug.Log("Setting Stats");

        _stats = characterStats;

        if (_stats.Length > _statDisplays.Length)
        {
            Debug.LogError("Not enough stat displays!");
        }
        
        for (int i = 0; i < _statDisplays.Length; i++)
        {
            _statDisplays[i].gameObject.SetActive(i < _stats.Length);
        }

        UpdateStatValues();
    }

    public void UpdateStatValues()
    {
        for (int i = 0; i < _stats.Length; i++) 
        {
            Debug.Log(_stats[i].Value);
            _statDisplays[i].StatValue.text = _stats[i].Value.ToString();
        }
    }
}

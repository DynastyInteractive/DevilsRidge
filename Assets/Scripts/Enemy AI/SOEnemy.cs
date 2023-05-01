using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum Stat
{
    enemyType,
    hitPoints,
    maxHitPoints,
    movementSpeed,
    damage
}

[CreateAssetMenu(fileName = "Enemy Unit", menuName = "SO_Enemy", order = 67)]
public class SOEnemy : ScriptableObject
{
    public enum EnemyType
    {
        Enemy_1,
        Enemy_2,
        Enemy_3,
    }

    public Dictionary<Stat, float> stats = new Dictionary<Stat, float>();
    public Dictionary<Stat, float> instanceStats = new Dictionary<Stat, float>();
    public List<StatInfo> statInfo = new List<StatInfo>();

    public void Initialise()
    {
        foreach (var stat in statInfo) stats.Add(stat.statType, stat.statValue);
    }

    public float GetStat(Stat stat)
    {
        if(stats.TryGetValue(stat, out float value)) return value;
        else return 0f;
    }

    public float ChangeStat(Stat stat, float amount)
    {
        if (stats.TryGetValue(stat, out float value))
        {
            stats[stat] += amount;
            return stats[stat];
        }
        else return -1f;
    }

    public EnemyType enemyType;
}

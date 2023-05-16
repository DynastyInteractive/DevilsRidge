using System.Collections;
using System.Collections.Generic;using UnityEngine;


[CreateAssetMenu(fileName = "Enemy Unit", menuName = "SO_Enemy", order = 67)]
public class SOEnemy : ScriptableObject
{
    public string enemyName;
    public MinMaxCharacterStat _healthPoints;
    public CharacterStat _strength;
    public CharacterStat _agility;

    public float targetRange;
    public float attackRange;

    public bool isBoss;

    public enum State
    {
        Idle,
        Wandering,
        Targeting,
        Attacking
    }

}

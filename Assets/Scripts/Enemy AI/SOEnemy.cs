using System.Collections;
using System.Collections.Generic;using UnityEngine;


[CreateAssetMenu(fileName = "Enemy Unit", menuName = "SO_Enemy", order = 67)]
public class SOEnemy : ScriptableObject
{
    public string enemyName;
    public int healthPoints;
    public float movementSpeed;
    public float damage;

    public enum State
    {
        Idle,
        Wandering,
        Targeting,
        Attacking
    }
}

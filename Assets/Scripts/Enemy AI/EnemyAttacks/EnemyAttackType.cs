using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttackType : MonoBehaviour
{
    [SerializeField] float _attackCooldown;

    public float AttackCooldown
    {
        get { return _attackCooldown; }
    }

    public enum AttackDistance
    {
        Short,
        Mid,
        Long,
    }

    public abstract void Attack(float damage);

    public virtual void GetCooldown(float attackCooldown)
    {
        _attackCooldown = attackCooldown;
        Debug.Log(attackCooldown);
    }

}

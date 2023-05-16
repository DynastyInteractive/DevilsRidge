using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttackType : MonoBehaviour
{
    [SerializeField] float _attackCooldown;
    [SerializeField] string _attackName;

    public float AttackCooldown
    {
        get { return _attackCooldown; }
        set { _attackCooldown = value;}
    }

    public string AttackName
    {
        get { return _attackName; }
        set { _attackName = value; }
    }

    public enum AttackDistance
    {
        Short,
        Mid,
        Long,
    }

    public abstract void Attack(float damage);

    public abstract void SetVariables();

    public virtual void GetCooldown(float attackCooldown, string attackName)
    {
        _attackCooldown = attackCooldown;
        _attackName = attackName;
        //Debug.Log(AttackCooldown);
        //Debug.Log(AttackName);
    }
}

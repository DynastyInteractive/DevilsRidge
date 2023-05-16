using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomp : EnemyAttackType
{
    [SerializeField] EnemyController _enemyController;

    [SerializeField] MinMaxCharacterStat _localAttackCooldown;
    [SerializeField] string _localAttackName;


    public void OnEnable()
    {
        _enemyController = GetComponent<EnemyController>();
        SetVariables();
        Attack(_enemyController.Enemy._strength);
        
        /*switch ((AttackDistance)Random.Range(0, (int)AttackDistance.Long + 1))
        {
            case AttackDistance.Short:
                //do animation
                break;
            case AttackDistance.Mid:
                //do nothing
                break;
            case AttackDistance.Long:
                //charge and then attack
                break;
            default:
                break;
        }*/
    }

    public override void SetVariables()
    {
        
    }

    public override void Attack(float damage)
    {
        Debug.Log("Stomp");
        //attack the player
        _enemyController.Animator?.SetTrigger("Stomp");

        for (int i = 0; i < _enemyController.PlayerPositions.Count; i++)
        {
            if (Vector3.Distance(transform.position, _enemyController.PlayerPositions[i]) < _enemyController.Enemy.attackRange)
            {
                Player playerStats = _enemyController.PlayerObjs[i].GetComponent<Player>();
                playerStats.TakeDamage(damage);
            }
        }

        _localAttackCooldown.SetCurrentValue(Random.Range(_localAttackCooldown.Min.BaseValue, _localAttackCooldown.Max.BaseValue));
        GetCooldown(_localAttackCooldown.CurrentValue, _localAttackName);
    }

    public override void GetCooldown(float attackCooldown, string attackName)
    {
        base.GetCooldown(attackCooldown, attackName);
    }
}

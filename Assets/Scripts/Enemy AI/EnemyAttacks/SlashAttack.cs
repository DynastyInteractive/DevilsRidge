using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : EnemyAttackType
{
    [SerializeField] EnemyController _enemyController;


    public virtual void OnEnable()
    {
        _enemyController = GetComponent<EnemyController>();
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

    public override void Attack(float damage)
    {
        Debug.Log("Slash Attack");
        //attack the player

        for (int i = 0; i < _enemyController.PlayerPositions.Count; i++)
        {
            if (Vector3.Distance(transform.position, _enemyController.PlayerPositions[i]) < _enemyController.Enemy.attackRange)
            {
                Player playerStats = _enemyController.PlayerObjs[i].GetComponent<Player>();
                playerStats.TakeDamage(damage);
            }
        }

        GetCooldown(2);
    }

    public override void GetCooldown(float attackCooldown)
    {
        base.GetCooldown(attackCooldown);

        this.enabled = false;
    }
}

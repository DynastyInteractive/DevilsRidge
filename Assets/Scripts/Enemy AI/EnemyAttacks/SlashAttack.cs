using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : EnemyAttack
{
    public override void Start() 
    {
        base.Start(); 
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void Attack(float damage)
    {
        //attack the player
        if (!CanAttack) return;

        CanAttack = false;

        for (int i = 0; i < EnemyController.PlayerPositions.Count; i++)
        {
            if (Vector3.Distance(transform.position, EnemyController.PlayerPositions[i]) < EnemyController.Enemy.attackRange)
            {
                Player playerStats = EnemyController.PlayerObjs[i].GetComponent<Player>();
                playerStats.TakeDamage(damage);
            }
        }

        StartCoroutine(AttackCooldown());
    }
}

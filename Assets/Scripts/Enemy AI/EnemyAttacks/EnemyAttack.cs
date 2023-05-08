using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] EnemyController _enemyController;

    // Start is called before the first frame update
    void Start()
    {
        _enemyController = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyController.State == SOEnemy.State.Attacking) Attack(_enemyController.Enemy.damage);
    }

    void Attack(float damage)
    {
        //attack the player
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] List<EnemyAttackType> attacks;
    EnemyController _enemyController;

    [SerializeField] bool canAttack;

    // Start is called before the first frame update
    void Start()
    {
        _enemyController = GetComponent<EnemyController>();
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_enemyController.State == SOEnemy.State.Attacking && canAttack)
        {
            canAttack = false;
            var randAttack = Random.Range(0, attacks.Count);
            Debug.Log(randAttack);
            attacks[randAttack].enabled = true;
            StartCoroutine(AttackCooldown(attacks[Random.Range(0, attacks.Count)].AttackCooldown));
        }
    }

    public IEnumerator AttackCooldown(float attackCooldown)
    {
        yield return new WaitForSeconds(attackCooldown);

        if (Random.Range(0, 5) == 0) canAttack = true;
    }
}

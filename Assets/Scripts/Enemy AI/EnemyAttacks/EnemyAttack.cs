using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] List<EnemyAttackType> attacks;
    EnemyController _enemyController;
    EnemyMovement _enemyMovement;

    [SerializeField] bool canAttack;
    [SerializeField] bool isCoolingDown;
    [SerializeField] int _randAttackNum;

    [SerializeField] float _rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _enemyController = GetComponent<EnemyController>();
        _enemyMovement = GetComponent<EnemyMovement>();
        canAttack = true;
        _randAttackNum = -1;

        foreach(EnemyAttackType type in attacks)
        {
            type.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _enemyMovement.enabled = true;


        foreach (EnemyAttackType type in attacks)
        {
            //if (_enemyController.Animator.GetCurrentAnimatorStateInfo(0).IsName(type.AttackName))
            //{
            //    _enemyMovement.enabled = false;
            //}
        }

        if(_enemyController.State == SOEnemy.State.Attacking)
        {
            if (canAttack)
            {
                _randAttackNum = Random.Range(0, attacks.Count);
                canAttack = false;
                //Debug.Log(randAttack);
                attacks[_randAttackNum].enabled = true;

                StartCoroutine(AttackCooldown(attacks[_randAttackNum].AttackCooldown + _enemyController.Animator.GetCurrentAnimatorStateInfo(0).length));
            }
            else
            {
                Vector3 dir = _enemyController.PlayerPositions[_enemyController.ClosestPlayerIndex];
                transform.LookAt(_enemyController.PlayerPositions[_enemyController.ClosestPlayerIndex]);
            }
        }
    }

    public IEnumerator AttackCooldown(float attackCooldown)
    {
        Debug.Log("Cooldown:" + attackCooldown);
        yield return new WaitForSeconds(attackCooldown);

        attacks[_randAttackNum].enabled = false;
        canAttack = true;
        isCoolingDown = false;
    }
}

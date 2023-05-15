using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    EnemyController _enemyController;

    [SerializeField] bool canAttack;
    [SerializeField] float attackCooldown = 2f;

    public EnemyController EnemyController
    {
        get { return EnemyController; }
    }

    public bool CanAttack
    {
        get { return canAttack; }
        set { canAttack = value; }
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        _enemyController = GetComponent<EnemyController>();
        canAttack = true;
    }

    public virtual void OnEnable()
    {
        Attack(_enemyController.Enemy._strength);
    }

    public abstract void Attack(float damage);

    public IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);

        if(Random.Range(0, 5) == 0) canAttack= true;
    }
}

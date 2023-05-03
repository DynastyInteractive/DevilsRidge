using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    EnemyController _enemyController;

    [SerializeField] int currentHealth;


    // Start is called before the first frame update
    void Start()
    {
        _enemyController = GetComponent<EnemyController>();
        currentHealth = _enemyController.Enemy.healthPoints;
    }

    /*private void Update()
    {
        if (_enemy.enemyName == "Enemy 1")
        {
            TakeDamage(1);
        }
    }*/

    public void TakeDamage(int damage)
    {
        if(currentHealth - damage < damage)
        {
            //death
        }
        else
        {
            currentHealth -= damage;
        }
    }
}

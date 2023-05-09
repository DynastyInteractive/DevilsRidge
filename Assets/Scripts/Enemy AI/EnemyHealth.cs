using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    EnemyController _enemyController;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;


    // Start is called before the first frame update
    void Start()
    {
        _enemyController = GetComponent<EnemyController>();
        currentHealth = _enemyController.Enemy.healthPoints;
        maxHealth = _enemyController.Enemy.healthPoints;
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
        if(currentHealth - damage < 0)
        {
            //death
        }
        else
        {
            currentHealth -= damage;
        }
    }

    public void Heal(int healthReturn)
    {
        if (currentHealth < 0) return;

        if(currentHealth + healthReturn > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}

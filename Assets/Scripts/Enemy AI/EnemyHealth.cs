using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    EnemyController _enemyController;
    BossFightManager _bossFightManager;

    // Start is called before the first frame update
    void Start()
    {
        _enemyController = GetComponent<EnemyController>();
        _enemyController.Enemy._healthPoints.SetCurrentValue(_enemyController.Enemy._healthPoints.Max.BaseValue);

        if (_enemyController.Enemy.isBoss)
        {
            _bossFightManager = _enemyController.NearestCamp.GetComponent<BossFightManager>();
            _bossFightManager?.SetMaxHealth(_enemyController.Enemy._healthPoints.Max.BaseValue);
        }
    }
    public void TakeDamage(float damage)
    {
        _enemyController.Enemy._healthPoints.SetCurrentValue(_enemyController.Enemy._healthPoints.CurrentValue - damage);

        if (_enemyController.Enemy._healthPoints.CurrentValue <= _enemyController.Enemy._healthPoints.Min)
        {
            //death
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            _bossFightManager?.EndBossFight(true, this);
            GameObject.Destroy(gameObject);
        }

        _bossFightManager?.SetHealth(_enemyController.Enemy._healthPoints.CurrentValue);
    }

    public void Heal(float healthReturn)
    {
        _enemyController.Enemy._healthPoints.SetCurrentValue(_enemyController.Enemy._healthPoints.CurrentValue + healthReturn);
        _bossFightManager?.SetHealth(_enemyController.Enemy._healthPoints.CurrentValue);
    }

    public void ResetHealth()
    {
        _enemyController.Enemy._healthPoints.SetCurrentValue(_enemyController.Enemy._healthPoints.Max.BaseValue);
        _bossFightManager?.SetMaxHealth(_enemyController.Enemy._healthPoints.Max.BaseValue);
    }
}

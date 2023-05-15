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

        if (_enemyController.Enemy._healthPoints.CurrentValue <= _enemyController.Enemy._healthPoints.Min) GameObject.Destroy(gameObject); //death

        _bossFightManager?.SetHealth(_enemyController.Enemy._healthPoints.CurrentValue);
    }

    public void Heal(int healthReturn)
    {
        _enemyController.Enemy._healthPoints.SetCurrentValue(_enemyController.Enemy._healthPoints.CurrentValue + healthReturn);
        _bossFightManager?.SetHealth(_enemyController.Enemy._healthPoints.CurrentValue);
    }
}

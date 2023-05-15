using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] MinMaxCharacterStat _healthPoints;
    [SerializeField] CharacterStat _strength;
    [SerializeField] CharacterStat _agility;


    // Start is called before the first frame update
    void Start()
    {
        _healthPoints.SetCurrentValue(_healthPoints.Max.BaseValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        _healthPoints.SetCurrentValue(_healthPoints.CurrentValue - damage);

        if (_healthPoints.CurrentValue <= _healthPoints.Min) GameObject.Destroy(gameObject); //death
    }

    public void Heal(int healthReturn)
    {
        _healthPoints.SetCurrentValue(_healthPoints.CurrentValue.BaseValue + healthReturn);
    }
}

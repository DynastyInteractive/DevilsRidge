using Unity.Netcode;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] MinMaxCharacterStat _healthPoints;
    [SerializeField] CharacterStat _strength;
    [SerializeField] CharacterStat _agility;
    [SerializeField] CharacterStat _critChancePercentage;
    [SerializeField] CharacterStat _critDamagePercentage;

    public MinMaxCharacterStat HealthPoints => _healthPoints;
    public CharacterStat Strength => _strength;
    public CharacterStat Agility => _agility;
    public CharacterStat CritChancePercentage => _critChancePercentage;
    public CharacterStat CritDamagePercentage => _critDamagePercentage;

    private void Start()
    {
        StatPanel.Instance.SetStats(_healthPoints.Max, _strength, _agility, _critChancePercentage, _critDamagePercentage);
    }

    public void TakeDamage(float damage)
    {
        _healthPoints.SetCurrentValue(_healthPoints.CurrentValue - damage);
    }

}

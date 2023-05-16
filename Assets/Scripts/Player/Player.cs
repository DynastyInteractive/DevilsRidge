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
        Debug.Log(NetworkManager.Singleton.ConnectedClientsList.Count);
        Debug.Log(_healthPoints.Max.Value);
        Debug.Log(_strength.Value);
        Debug.Log(_agility.Value);
        Debug.Log(_critChancePercentage.Value);
        Debug.Log(_critDamagePercentage.Value);
        StatPanel.Instance.SetStats(_healthPoints.Max, _strength, _agility, _critChancePercentage, _critDamagePercentage);
    }

    public void TakeDamage(float damage)
    {
        _healthPoints.SetCurrentValue(_healthPoints.CurrentValue - damage);
    }

}

using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] Transform _weaponHolder;

    [Header("Stats")]
    [SerializeField] MinMaxCharacterStat _healthPoints;
    [SerializeField] CharacterStat _strength;
    [SerializeField] CharacterStat _agility;

    public MinMaxCharacterStat HealthPoints => _healthPoints;
    public CharacterStat Strength => _strength;
    public CharacterStat Agility => _agility;

    public void EquipWeapon(GameObject weapon)
    {
        var weaponGO = Instantiate(weapon, _weaponHolder);
        weaponGO.transform.localPosition = Vector3.zero;
        weaponGO.transform.localRotation = Quaternion.identity;
    }

    public void TakeDamage(float damage)
    {
        _healthPoints.SetCurrentValue(_healthPoints.CurrentValue - damage);
    }

}

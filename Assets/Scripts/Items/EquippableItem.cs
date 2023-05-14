using System;
using UnityEngine;

public enum EquippableType
{
    Weapon,
    MagicWeapon,
    Talisman
}

[CreateAssetMenu(fileName = "Equippable Item", menuName = "Items/Equippable Item")]
public class EquippableItem : Item
{
    [SerializeField] EquippableType _type = EquippableType.Weapon;

    [SerializeField] float _maxHealthBonus;
    [SerializeField] float _strengthBonus;
    [SerializeField] float _agilityBonus;
    [Space]
    [SerializeField] float _maxHealthPercentBonus;
    [SerializeField] float _strengthPercentBonus;
    [SerializeField] float _agiltyPercentBonus;

    public EquippableType Type => _type;
    public float MaxHealthBonus => _maxHealthBonus;
    public float StrengthBonus => _strengthBonus;
    public float AgilityBonus => _agilityBonus; 
    public float MaxHealthPercentBonus => _maxHealthPercentBonus;
    public float StrengthPercentBonus => _strengthPercentBonus;
    public float AgilityPercentBonus => _agiltyPercentBonus;

    public event Action OnEquipped;
    public event Action OnUnequipped;

    public virtual void Equip(Player player)
    {
        if (MaxHealthBonus != 0) player.HealthPoints.Max.AddModifier(new StatModifier(MaxHealthBonus, StatModifierType.Flat, this));
        if (StrengthBonus != 0) player.Strength.AddModifier(new StatModifier(StrengthBonus, StatModifierType.Flat, this));
        if (AgilityBonus != 0) player.Agility.AddModifier(new StatModifier(AgilityBonus, StatModifierType.Flat, this));
        
        if (MaxHealthPercentBonus != 0) player.HealthPoints.Max.AddModifier(new StatModifier(MaxHealthPercentBonus, StatModifierType.PercentAdd, this));
        if (StrengthPercentBonus != 0) player.Strength.AddModifier(new StatModifier(StrengthPercentBonus, StatModifierType.PercentAdd, this));
        if (AgilityPercentBonus != 0) player.Agility.AddModifier(new StatModifier(AgilityPercentBonus, StatModifierType.PercentAdd, this));

        OnEquipped?.Invoke();
    }

    public virtual void Unequip(Player player)
    {
        player.HealthPoints.Max.RemoveAllModifiersFromSource(this);
        player.Strength.RemoveAllModifiersFromSource(this);
        player.Agility.RemoveAllModifiersFromSource(this);

        OnUnequipped?.Invoke();
    }
}

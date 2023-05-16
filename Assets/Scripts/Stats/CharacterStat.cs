using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public class CharacterStat
{
    [SerializeField] float _baseValue;

    public virtual float BaseValue
    {
        get => _baseValue;
        set
        {
            _baseValue = value;
            _isDirty = true;
        }
    }

    public virtual float Value {
        get
        {
            if (_isDirty)
            {
                _value = CalculateFinalValue();
                _isDirty = false;
            }
            return _value;
        }
    }

    protected bool _isDirty = true;
    protected float _value;

    protected readonly List<StatModifier> statModifiers;
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;

    public static implicit operator float(CharacterStat stat) => stat.Value;

    public CharacterStat()
    {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }

    public CharacterStat(float baseValue) : this()
    {
        BaseValue = baseValue;
    }

    public virtual void AddModifier(StatModifier modifier)
    {
        statModifiers.Add(modifier);
        statModifiers.Sort(CompareModifierOrder);
        _isDirty = true;
    }

    protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.Order < b.Order) return -1;
        else if (a.Order > b.Order) return 1;
        return 0;
    }

    public virtual void RemoveModifier(StatModifier modifier)
    {
        if (statModifiers.Remove(modifier)) _isDirty = true;
    }

    public virtual bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;

        for (int i = statModifiers.Count - 1; i >= 0; i--) 
        {
            if (statModifiers[i].Source == source)
            {
                statModifiers.RemoveAt(i);
                _isDirty = true;
                didRemove = true;
            }
        }

        return didRemove;
    }

    protected virtual float CalculateFinalValue()
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;
        Debug.Log("Final Value 1:" + finalValue);

        for (int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier modifier = statModifiers[i];

            if (modifier.Type == StatModifierType.Flat)
            {
                finalValue += modifier.Value;
            }
            else if (modifier.Type == StatModifierType.PercentAdd)
            {
                sumPercentAdd += modifier.Value;

                if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModifierType.PercentAdd)
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if (modifier.Type == StatModifierType.PercentMultiply)
            {
                finalValue *= 1 + modifier.Value;
            }
        }


        Debug.Log("Final Value 2:" + finalValue);
        return (float)Math.Round(finalValue, 4);
    }
}

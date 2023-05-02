using System;
using UnityEngine;

[Serializable]
public class MinMaxCharacterStat
{
    [SerializeField] CharacterStat _min;
    [SerializeField] CharacterStat _max;
    [SerializeField] CharacterStat _currentValue;

    public CharacterStat Min => _min;
    public CharacterStat Max => _max;
    public CharacterStat CurrentValue => _currentValue;

    public void SetMin(float value)
    {
        Min.BaseValue = value;
    }

    public void SetMax(float value)
    {
        Max.BaseValue = value;
    }

    public void SetCurrentValue(float value)
    {
        if (value < Min.BaseValue) value = Min.BaseValue;
        if (value > Max.BaseValue) value = Max.BaseValue;

        CurrentValue.BaseValue = value;
    }
}

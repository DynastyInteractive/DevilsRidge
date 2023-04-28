using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField]
    float _maxHealth = 100;

    [SerializeField]
    float _speed = 10;

    [SerializeField]
    float _strength = 10;

    [SerializeField]
    StatusEffect[] _statusEffects;

    EventList<StatusEffect> _statusEffectsList;

    float _currentHealth;

    void Awake()
    {
        _statusEffectsList = new EventList<StatusEffect>(_statusEffects.ToList());



        _currentHealth = _maxHealth;
    }

    public float CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }

    public float MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }

    public float Speed { get { return _speed; } set { _speed = value; } }

    public float Strength { get { return _strength; } set { _strength = value; } }
}

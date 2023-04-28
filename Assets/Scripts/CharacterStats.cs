using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField]
    float _maxHealth = 100;

    [SerializeField]
    float _speed = 10;

    [SerializeField]
    float _strength = 10;

    public float MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }

    public float Speed { get { return _speed; } set { _speed = value; } }

    public float Strength { get { return _strength; } set { _strength = value; } }
}

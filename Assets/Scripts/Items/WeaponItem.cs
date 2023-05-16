using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon")]
public class WeaponItem : EquippableItem
{
    [SerializeField] GameObject _weaponPrefab;

    public GameObject WeaponPrefab => _weaponPrefab;
}

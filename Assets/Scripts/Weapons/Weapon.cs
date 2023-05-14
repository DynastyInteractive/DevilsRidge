using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponItem WeaponData { private get; set; }

    public void Equip(Player player)
    {
        WeaponData.Equip(player);
    }

    public void Unequip(Player player)
    {
        WeaponData.Unequip(player);
    }
}

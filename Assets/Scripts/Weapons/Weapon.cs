using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponItem WeaponData { private get; set; }

    public void Equip(Player player)
    {
        Debug.Log("Weapon Equip");
        WeaponData.Equip(player);
        StatPanel.Instance.UpdateStatValues();
    }

    public void Unequip(Player player)
    {
        WeaponData.Unequip(player);
        StatPanel.Instance.UpdateStatValues();
    }
}

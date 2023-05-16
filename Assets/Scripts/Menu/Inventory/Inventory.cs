using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] int _maxInventorySize;
    [SerializeField] int _maxEquipmentSize;

    List<Item> _items = new List<Item>();
    WeaponItem[] _weapons = new WeaponItem[2];
    TalismanItem[] _talisman = new TalismanItem[2];

    public List<Item> Items => _items;
    public WeaponItem[] Weapons => _weapons;
    public TalismanItem[] Talisman => _talisman;

    public static Inventory Instance;

    public event Action<Item, bool> OnInventoryChanged;
    public event Action<WeaponItem, bool> OnWeaponAdded;
    public event Action<WeaponItem, int> OnWeaponRemoved; 
    public event Action<TalismanItem> OnTalismanEquipped;
    public event Action<TalismanItem> OnTalismanUnequipped;

    public Item LastItemAdded { get; set; }


    private void Awake()
    {
        Debug.Log("Inventory: " + Time.time);
        Instance = this;
    }
    
    public void EquipWeapon(WeaponItem weapon, bool wasDropped)
    {
        if (_weapons[0] != null && _weapons[1] != null)
        {
            Debug.LogWarning("Equipment Full!");
            return;
        }

        for (int i = 0; i < _weapons.Length; i++)
        {
            if (_weapons[i] == null)
            {
                _weapons[i] = weapon;
                break;
            }
        }

        OnWeaponAdded?.Invoke(weapon, wasDropped);
    }

    public void UnequipWeapon(WeaponItem weapon)
    {
        int index = -1;
        for (int i = 0; i < _weapons.Length; i++)
        {
            Debug.Log(_weapons[i].ItemName);
            Debug.Log(weapon.ItemName);
            if (_weapons[i].ItemName == weapon.ItemName)
            {
                _weapons[i] = null;
                index = i;
                OnWeaponRemoved.Invoke(weapon, index);
                break;
            }
        }
        Debug.Log(index);
    }
    
    public void EquipTalisman(TalismanItem talisman, bool wasDropped)
    {
        if (_talisman[0] != null && _talisman[1] != null)
        {
            Debug.LogWarning("Equipment Full!");
            return;
        }

        for (int i = 0; i < _talisman.Length; i++)
        {
            if (_weapons[i] == null)
            {
                _talisman[i] = talisman;
                break;
            }
        }
        if (!wasDropped) OnTalismanEquipped?.Invoke(talisman);
    }

    public void UnequipTalisman(TalismanItem talisman)
    {
        int index = _talisman.ToList().IndexOf(talisman);
        _talisman[index] = null;
        OnTalismanUnequipped.Invoke(talisman);
    }

    public void Add(Item item, bool wasDropped)
    {
        if (_items.Count >= _maxInventorySize)
        {
            Debug.LogWarning("Inventory Full!");
            return;
        }

        _items.Add(item);
        LastItemAdded = item;
        if (!wasDropped) OnInventoryChanged?.Invoke(item, true);
    }

    public void Remove(Item item)
    {
        _items.Remove(item);
        OnInventoryChanged?.Invoke(item, false);
    }
}

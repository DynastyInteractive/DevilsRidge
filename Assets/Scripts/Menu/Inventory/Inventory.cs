using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] int _maxInventorySize;
    [SerializeField] int _maxEquipmentSize;

    List<Item> _items = new List<Item>();
    List<EquippableItem> _equippedItems = new List<EquippableItem>();

    public List<Item> Items => _items;
    public List<EquippableItem> EquippedItems => _equippedItems;

    public static Inventory Instance;

    public event Action<Item, bool> OnInventoryChanged;
    public event Action<EquippableItem, bool> OnEquipmentChanged;

    private void Awake()
    {
        Instance = this;
    }
    
    public void Equip(EquippableItem equippable)
    {
        if (_equippedItems.Count >= _maxEquipmentSize)
        {
            Debug.LogWarning("Equipment Full!");
            return;
        }

        _equippedItems.Add(equippable);
        OnEquipmentChanged?.Invoke(equippable, true);
    }

    public void Unequip(EquippableItem equippable)
    {
        _equippedItems.Remove(equippable);
        OnEquipmentChanged.Invoke(equippable, false);
    }

    public void Add (Item item)
    {
        if (_items.Count >= _maxInventorySize)
        {
            Debug.LogWarning("Inventory Full!");
            return;
        }

        _items.Add(item);
        OnInventoryChanged?.Invoke(item, true);
    }

    public void Remove(Item item)
    {
        _items.Remove(item);
        OnInventoryChanged?.Invoke(item, false);
    }
}

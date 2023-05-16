
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Transform _itemsParent;
    [SerializeField] Transform _weaponSlotParent;
    [SerializeField] Transform _talismanSlotParent;

    Inventory inventory;

    InventorySlot[] _slots;
    WeaponSlot[] _weaponSlots;
    WeaponSlot[] _talismanSlots;

    void OnEnable()
    {
        Invoke(nameof(SubscribeToInventory), .2f);


        _slots = _itemsParent.GetComponentsInChildren<InventorySlot>();
        _weaponSlots = _weaponSlotParent.GetComponentsInChildren<WeaponSlot>();
        _talismanSlots = _talismanSlotParent.GetComponentsInChildren<WeaponSlot>();
    }

    void SubscribeToInventory()
    {
        inventory = Inventory.Instance;

        UpdateUI();
        inventory.OnInventoryChanged += RefreshUI;
        inventory.OnWeaponAdded += RefreshWeaponUIAdded;
        inventory.OnWeaponRemoved += RefreshWeaponUIRemoved;
        inventory.OnTalismanEquipped += RefreshTalismanUIAdded;
        inventory.OnTalismanUnequipped += RefreshTalismanUIRemoved;
    }

    void OnDisable()
    {
        inventory.OnInventoryChanged -= RefreshUI;
        inventory.OnWeaponAdded -= RefreshWeaponUIAdded;
        inventory.OnWeaponRemoved -= RefreshWeaponUIRemoved;
        inventory.OnTalismanEquipped -= RefreshTalismanUIAdded;
        inventory.OnTalismanUnequipped -= RefreshTalismanUIRemoved;
    }

    void UpdateUI()
    {
        foreach (var item in inventory.Items)
        {
            bool isContained = false;
            foreach (var slot in _slots)
            {
                if (slot.CurrentItem == item) 
                { 
                    isContained = true; 
                    break;
                }
            }
            if (!isContained) RefreshUI(item, true);
        }
    }

    void RefreshUI(Item itemChanged, bool wasAdded)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (wasAdded && _slots[i].CurrentItem == null)
            { 
                _slots[i].AddItem(itemChanged); 
                return;
            }
            if (!wasAdded && _slots[i].CurrentItem == itemChanged)
            {
                _slots[i].ClearSlot();
                Debug.Log("Clearing Slot");
                return;
            }
        }
    }

    void RefreshWeaponUIAdded(WeaponItem itemChanged, bool wasDropped)
    {
        if (wasDropped) return;

        for (int i = 0; i < _weaponSlots.Length; i++)
        {
            if (_weaponSlots[i].CurrentItem == null)
            {
                _weaponSlots[i].AddItem(itemChanged);
                return;
            }
        }
    }
    void RefreshWeaponUIRemoved(WeaponItem itemChanged, int slot)
    {
        Debug.Log(_weaponSlots[slot].gameObject.name);
        _weaponSlots[slot].ClearSlot();
    }

    void RefreshTalismanUIAdded(TalismanItem itemChanged)
    {
        for (int i = 0; i < _talismanSlots.Length; i++)
        {
            if (_talismanSlots[i].CurrentItem == null)
            {
                _talismanSlots[i].AddItem(itemChanged);
                return;
            }
        }
    }
    
    void RefreshTalismanUIRemoved(TalismanItem itemChanged)
    {
        for (int i = 0; i < _talismanSlots.Length; i++)
        {
            if (_talismanSlots[i].CurrentItem == itemChanged)
            {
                _slots[i].ClearSlot();
                return;
            }
        }
    }
}


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
    EquipmentSlot[] _weaponSlots;
    EquipmentSlot[] _talismanSlots;

    void Start()
    {
        inventory = Inventory.Instance;
        inventory.OnInventoryChanged += RefreshUI;
        inventory.OnEquipmentChanged += RefreshEquipmentUI;

        _slots = _itemsParent.GetComponentsInChildren<InventorySlot>();
        _weaponSlots = _weaponSlotParent.GetComponentsInChildren<EquipmentSlot>();
        _talismanSlots = _talismanSlotParent.GetComponentsInChildren<EquipmentSlot>();
    }

    void RefreshUI(Item itemChanged, bool wasAdded)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (wasAdded && _slots[i].CurrentItem != itemChanged)
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

    void RefreshEquipmentUI(EquippableItem itemChanged, bool wasAdded)
    {
        for (int i = 0; i < _weaponSlots.Length; i++)
        {
            if (wasAdded && _weaponSlots[i].CurrentItem != itemChanged)
            {
                _weaponSlots[i].AddItem(itemChanged);
                return;
            }

            if (!wasAdded && _talismanSlots[i].CurrentItem == itemChanged)
            {
                inventory.Add(itemChanged);
                return;
            }
        }
        
        for (int i = 0; i < _talismanSlots.Length; i++)
        {
            if (wasAdded && _talismanSlots[i].CurrentItem != itemChanged)
            {
                _talismanSlots[i].AddItem(itemChanged);
                return;
            }

            if (!wasAdded && _talismanSlots[i].CurrentItem == itemChanged)
            {
                inventory.Add(itemChanged);
                return;
            }
        }
    }
}

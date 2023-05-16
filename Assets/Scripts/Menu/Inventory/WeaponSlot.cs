using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlot : InventorySlot
{
    [SerializeField] EquippableType _acceptedType;

    public override void OnDrop(PointerEventData eventData)
    {
        if (!eventData.pointerDrag.TryGetComponent(out DraggableItem draggableItem)) return;

        if (transform.childCount != 0) return;

        var weapon = draggableItem.Item as WeaponItem;

        if (weapon == null) return;

        if (weapon.Type != _acceptedType) return;

        Inventory.Instance.EquipWeapon(weapon, true);
        draggableItem.ParentAfterDrag = transform;
        _item = draggableItem.Item;
        _removeButton.interactable = true;
    }

    public override void OnRemoveButton()
    {
        Inventory.Instance.UnequipWeapon(_item as WeaponItem);
    }
}

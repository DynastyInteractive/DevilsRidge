using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : InventorySlot
{
    [SerializeField] EquippableType _acceptedType;

    public override void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (!eventData.pointerDrag.TryGetComponent(out DraggableItem draggableItem)) return;

        Debug.Log("DraggableItem");
        if (transform.childCount != 0) return;
        Debug.Log("Empty Slot");
        var equippable = draggableItem.Item as EquippableItem;
        if (equippable == null) return;

        Debug.Log("EquippableItem");
        if (equippable.Type != _acceptedType) return;
        Debug.Log("Accepted Type");

        if (!Inventory.Instance.EquippedItems.Contains(equippable)) Inventory.Instance.Add(_item);
        draggableItem.ParentAfterDrag = transform;
        _item = draggableItem.Item;
        _removeButton.interactable = true;
    }

    public override void OnRemoveButton()
    {
        Inventory.Instance.Unequip((EquippableItem)_item);
        Inventory.Instance.Add(_item);
    }

    public override void ClearSlot()
    {
        DetachSlot();
        Inventory.Instance.Unequip((EquippableItem)_item);
    }
}

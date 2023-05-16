using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class TalismanSlot : InventorySlot
{
    [SerializeField] EquippableType _acceptedType;

    public override void OnDrop(PointerEventData eventData)
    {
        if (!eventData.pointerDrag.TryGetComponent(out DraggableItem draggableItem)) return;

        if (transform.childCount != 0) return;

        var equippable = draggableItem.Item as EquippableItem;

        if (equippable == null) return;

        if (equippable.Type != _acceptedType) return;

        if (!Inventory.Instance.Weapons.ToList().Contains(equippable)) Inventory.Instance.Add(_item, true);
        draggableItem.ParentAfterDrag = transform;
        _item = draggableItem.Item;
        _draggableItem = draggableItem;
        _removeButton.interactable = true;
    }
}

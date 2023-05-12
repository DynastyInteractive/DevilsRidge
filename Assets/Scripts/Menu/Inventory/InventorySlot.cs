using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] protected Button _removeButton;
    [SerializeField] protected DraggableItem _draggableItemPrefab;

    protected DraggableItem _draggableItem;
    
    protected Item _item;

    public Item CurrentItem => _item;

    protected virtual void Awake()
    {
        _removeButton?.onClick.AddListener(OnRemoveButton);
    }

    public virtual void AddItem(Item newItem)
    {
        _item = newItem;

        _draggableItem = (_draggableItem != null) ? _draggableItem : Instantiate(_draggableItemPrefab, transform);
        _draggableItem.Item = _item;
        _draggableItem.Icon = _item.Icon;
        _removeButton.interactable = true;
    }

    public virtual void DetachSlot()
    {
        _item = null;
        _draggableItem = null;
        _removeButton.interactable = false;
    }

    public virtual void ClearSlot()
    {
        DetachSlot();
        Destroy(transform.GetChild(0).gameObject);
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (!eventData.pointerDrag.TryGetComponent(out DraggableItem draggableItem)) return;

        if (transform.childCount == 0)
        {
            if (!Inventory.Instance.Items.Contains(_item)) Inventory.Instance.Add(_item);
            draggableItem.ParentAfterDrag = transform;
            _item = draggableItem.Item;
            _removeButton.interactable = true;
        }
    }

    public virtual void OnRemoveButton()
    {
        Inventory.Instance.Remove(_item);
    }
}

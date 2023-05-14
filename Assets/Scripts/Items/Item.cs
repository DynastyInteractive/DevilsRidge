using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    [SerializeField] Sprite _icon;
    [SerializeField] string _itemName;

    public Sprite Icon => _icon;
    public string ItemName => _itemName;
}

using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentItem", menuName = "Items/CurrentItem")]
public class Item : ScriptableObject
{
    [SerializeField] Sprite _icon;
    [SerializeField] string _itemName;

    public Sprite Icon => _icon;
    public string ItemName => _itemName;
}

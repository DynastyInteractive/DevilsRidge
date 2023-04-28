using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public event Action OnItemAdded;

    private void Awake()
    {
        instance = this;
    }
    
    public List<Item> items = new List<Item>();

    public void Add (Item item)
    {
        items.Add(item);
        OnItemAdded?.Invoke();
        Debug.LogWarning("More than one instance of Inventory found!");
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }
}

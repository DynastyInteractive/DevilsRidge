using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventorysyterm : MonoBehaviour
{


    public List<Item> items = new List<Item>();
    public void Add (Item item)
    {
        items.Add(item);
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }
}

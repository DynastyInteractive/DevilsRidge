using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    public GameObject lootPrefab;
    public Material rarity;
    public string lootName;
    public int dropChance;

    public Loot(GameObject lootPrefab, Material rarity, string lootName, int dropChance)
    {
        this.lootPrefab = lootPrefab;
        this.rarity = rarity;
        this.lootName = lootName;
        this.dropChance = dropChance;
    }
}

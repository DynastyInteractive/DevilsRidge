using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    [SerializeField] GameObject lootPrefab;
    [SerializeField] Material rarity;
    [SerializeField] string lootName;
    [SerializeField] float dropChance;

    public GameObject LootPrefab => lootPrefab;
    public Material Rarity => rarity;
    public string LootName => lootName;
    public float DropChance => dropChance;
    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    [SerializeField] GameObject _droppedItemPrefab;
    [SerializeField] List<Loot> _lootList = new List<Loot>(); // holds possible drops
    [SerializeField] float _uniqueDropChance;
    [SerializeField] List<Loot> _uniqueLootList = new List<Loot>(); // holds unique loot

    List<Loot> GetDroppedItems() // determines items dropped
    {
        int randomNumber = Random.Range(1, 101); // 1-100 drop weight
        Debug.Log("Weight: "+randomNumber.ToString());

        if (randomNumber <= _uniqueDropChance)
        {
            return _uniqueLootList;
        }
        List<Loot> possibleItems = new List<Loot>();
        foreach (Loot item in _lootList) // iterates through possible items
        {
            if(randomNumber <= item.DropChance) // checks if item is within weight
            {
                possibleItems.Add(item);
            }
        }
        if (possibleItems.Count > 0) 
        {
            Debug.Log("PossibleNo: "+possibleItems.Count.ToString());
            int itemNumber = Random.Range(1, possibleItems.Count+1); // amount of items dropped
            Debug.Log("AmountDrop: "+itemNumber.ToString());

            List<Loot> droppedItems = new List<Loot>();

            while (possibleItems.Count > 0) // shuffles possible items
            {
                Loot newItem = possibleItems[Random.Range(0, possibleItems.Count-1)];
                droppedItems.Add(newItem);
                possibleItems.Remove(newItem);
            }

            droppedItems = droppedItems.GetRange(0, itemNumber);
            return droppedItems; // returns items that will be dropped
        }
        Debug.Log("No Loot Dropped");
        return null; // if no drops
        
    }

    public void InstantiateLoot(Vector3 spawnPosition) // create loot
    {
        List<Loot> droppedItems = GetDroppedItems(); 
        if (droppedItems != null)
        {
            foreach (Loot item in droppedItems) // instantiates all dropped items
            {
                GameObject lootGameObject = Instantiate(_droppedItemPrefab, spawnPosition, Quaternion.identity);
                lootGameObject.GetComponent<Renderer>().material = item.Rarity;

                float dropForce = 3f;
                Vector3 dropDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f,1f));
                lootGameObject.GetComponent<Rigidbody>().AddForce(dropDirection * dropForce, ForceMode.Impulse);
            }
        }
    }
}

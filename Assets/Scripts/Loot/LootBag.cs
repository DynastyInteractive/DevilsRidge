using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    [SerializeField] GameObject _droppedItemPrefab;
    [SerializeField] List<Loot> _lootList = new List<Loot>(); // holds possible drops

    List<Loot> GetDroppedItems() // determines items dropped
    {
        int randomNumber = Random.Range(1, 101); // 1-100 drop weight
        Debug.Log("Weight: "+randomNumber.ToString());

        List<Loot> possibleItems = new List<Loot>();
        foreach (Loot item in _lootList) // iterates through possible items
        {
            if(randomNumber <= item.dropChance) // checks if item is within weight
            {
                possibleItems.Add(item);
            }
        }
        if (possibleItems.Count > 0) 
        {
            Debug.Log("PossibleNo: "+possibleItems.Count.ToString());
            int itemNumber = Random.Range(1, possibleItems.Count+1); // amount of items dropped
            Debug.Log("AmountDrop: "+itemNumber.ToString());

            // NOTE TO SELF - Shuffle possibleItems list here

            List<Loot> droppedItems = possibleItems.GetRange(0,itemNumber); // list of what items dropped
            return droppedItems;
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
                lootGameObject.GetComponent<Renderer>().material = item.rarity;

                float dropForce = 1f;
                Vector3 dropDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f,1f));
                lootGameObject.GetComponent<Rigidbody>().AddForce(dropDirection * dropForce, ForceMode.Impulse);
            }
        }
    }
}

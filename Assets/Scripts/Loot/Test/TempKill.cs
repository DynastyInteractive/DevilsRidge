using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempKill : MonoBehaviour
{
    public void Die()
    {
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        Destroy(gameObject);
    }
}

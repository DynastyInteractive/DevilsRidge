using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float campRadius;
    [SerializeField] GameObject[] enemies;
    [SerializeField] int[] numbers;

    public float CampRadius
    {
        get { return campRadius; }
    }

    private void Start()
    {
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        foreach(GameObject enemy in enemies)
        {
            for(int i = 0; i < numbers[System.Array.IndexOf(enemies, enemy)]; i++)
            {
                Instantiate(enemy, new Vector3(Random.Range(transform.position.x - campRadius, transform.position.x + campRadius), 1.01f, Random.Range(transform.position.z - campRadius, transform.position.z + campRadius)), Quaternion.identity);
            }
        }
    }
}

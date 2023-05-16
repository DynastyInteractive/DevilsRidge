using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float campRadius;
    [SerializeField] GameObject[] enemies;
    [SerializeField] int[] numbers;
    [SerializeField] float spawnArea;

    public float CampRadius
    {
        get { return campRadius; }
    }

    public GameObject[] Enemies
    {
        get { return enemies; }
    }

    private void Start()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        if(col != null) col.radius = campRadius;
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        foreach(GameObject enemy in enemies)
        {
            for(int i = 0; i < numbers[System.Array.IndexOf(enemies, enemy)];)
            {
                EnemyController _enemyController = enemy.GetComponent<EnemyController>();

                if (_enemyController.Enemy.isBoss) spawnArea = campRadius / 2;
                else spawnArea = campRadius;

                Vector3 randomPos = new Vector3(Random.Range(transform.position.x - spawnArea, transform.position.x + spawnArea), 1.01f, Random.Range(transform.position.z - spawnArea, transform.position.z + spawnArea));

                if (Vector3.Distance(randomPos, transform.position) <= spawnArea)
                {
                    GameObject enemyObj = Instantiate(enemy, randomPos, Quaternion.identity);
                    EnemyController _enemyCont = enemyObj.GetComponent<EnemyController>();
                    _enemyCont.NearestCamp = gameObject;
                    i++;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, campRadius);
    }
}

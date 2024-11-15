using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject cpu; // Reference to CPU
    public float spawnInterval = 2f; // Interval between spawns

    private float nextSpawnTime;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        if (cpu!=null)
        {
            // Randomly choose an edge
            Vector3 spawnPosition = Vector3.zero;
            int edge = Random.Range(0, 4); // 0 = North, 1 = South, 2 = East, 3 = West

            switch (edge)
            {
                case 0: // North edge
                    spawnPosition = new Vector3(Random.Range(-5, 5), 0, 15);
                    break;
                case 1: // South edge
                    spawnPosition = new Vector3(Random.Range(-5, 5), 0, -15);
                    break;
                case 2: // East edge
                    spawnPosition = new Vector3(15, 0, Random.Range(-5, 5));
                    break;
                case 3: // West edge
                    spawnPosition = new Vector3(-15, 0, Random.Range(-5, 5));
                    break;
            }

            GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPosition, Quaternion.identity);
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.targetCPU = cpu; // Set the CPU as the enemy's target
        }
        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class EnemySpawner : MonoBehaviour
//{
//    public GameObject[] enemyPrefabs;
//    public GameObject cpu; // Reference to CPU
//    public float spawnInterval = 2f; // Interval between spawns

//    private float nextSpawnTime;

//    void Update()
//    {
//        if (Time.time >= nextSpawnTime)
//        {
//            SpawnEnemy();
//            nextSpawnTime = Time.time + spawnInterval;
//        }
//    }

//    void SpawnEnemy()
//    {
//        if (cpu!=null)
//        {
//            // Randomly choose an edge
//            Vector3 spawnPosition = Vector3.zero;
//            int edge = Random.Range(0, 4); // 0 = North, 1 = South, 2 = East, 3 = West

//            switch (edge)
//            {
//                case 0: // North edge
//                    spawnPosition = new Vector3(Random.Range(-5, 5), 0, 30);
//                    break;
//                case 1: // South edge
//                    spawnPosition = new Vector3(Random.Range(-5, 5), 0, -30);
//                    break;
//                case 2: // East edge
//                    spawnPosition = new Vector3(30, 0, Random.Range(-5, 5));
//                    break;
//                case 3: // West edge
//                    spawnPosition = new Vector3(-30, 0, Random.Range(-5, 5));
//                    break;
//            }

//            GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPosition, Quaternion.identity);
//            EnemyController enemyController = enemy.GetComponent<EnemyController>();
//            enemyController.targetCPU = cpu; // Set the CPU as the enemy's target
//        }

//    }
//}

//public class EnemySpawner : MonoBehaviour
//{
//    public GameObject[] enemyPrefabs;
//    public GameObject cpu; // Reference to CPU
//    public float spawnInterval = 2f; // Interval between spawns
//    public float spawnIntervalIncrement = 0.6f; // How much to decrease the spawn interval each time
//    public float IncrementInSeconds = 10f;

//    private float nextSpawnTime;
//    private float elapsedTime = 0f; // Timer to track the passage of time

//    void Update()
//    {
//        elapsedTime += Time.deltaTime; // Track the elapsed time since the start

//        // Every x seconds, reduce the spawn interval
//        if (elapsedTime >= IncrementInSeconds)
//        {
//            spawnInterval = Mathf.Max(0.1f, spawnInterval + spawnIntervalIncrement); // Ensure spawnInterval doesn't go below 0.1f
//            elapsedTime = 0f; // Reset elapsed time
//        }

//        // Spawn enemies based on the current spawn interval
//        if (Time.time >= nextSpawnTime)
//        {
//            SpawnEnemy();
//            nextSpawnTime = Time.time + spawnInterval; // Update next spawn time
//        }
//    }

//    void SpawnEnemy()
//    {
//        if (cpu != null)
//        {
//            // Randomly choose an edge
//            Vector3 spawnPosition = Vector3.zero;
//            int edge = Random.Range(0, 4); // 0 = North, 1 = South, 2 = East, 3 = West

//            switch (edge)
//            {
//                case 0: // North edge
//                    spawnPosition = new Vector3(Random.Range(-5, 5), 0, 30);
//                    break;
//                case 1: // South edge
//                    spawnPosition = new Vector3(Random.Range(-5, 5), 0, -30);
//                    break;
//                case 2: // East edge
//                    spawnPosition = new Vector3(30, 0, Random.Range(-5, 5));
//                    break;
//                case 3: // West edge
//                    spawnPosition = new Vector3(-30, 0, Random.Range(-5, 5));
//                    break;
//            }

//            GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPosition, Quaternion.identity);
//            EnemyController enemyController = enemy.GetComponent<EnemyController>();
//            enemyController.targetCPU = cpu; // Set the CPU as the enemy's target
//        }
//    }
//}

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] bossEnemyPrefabs;
    public GameObject cpu; // Reference to CPU
    public float spawnInterval = 2f; // Interval between spawns
    public float spawnIntervalDecrement = 0.2f; // How much to decrease the spawn interval each time
    public float dicrementInterval = 10f; // How often the spawn interval decreases

    private float nextSpawnTime;
    private float elapsedTime = 0f; // Timer to track the passage of time
    private bool isBossSpawned = false;

    void Update()
    {
        elapsedTime += Time.deltaTime; // Track the elapsed time since the start

        // Every x seconds, decrease the spawn interval
        if (elapsedTime >= dicrementInterval)
        {
            // Decrease the spawn interval but don't go below a minimum value (0.1f)
            spawnInterval = Mathf.Max(0.1f, spawnInterval - spawnIntervalDecrement);
            elapsedTime = 0f; // Reset elapsed time after decrement
        }
        if (isBossSpawned==false && spawnInterval<1f)
        {
            SpawnBossEnemy();
            isBossSpawned = true;
        }

        // Spawn enemies based on the current spawn interval
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval; // Update next spawn time
        }
    }

    void SpawnEnemy()
    {
        if (cpu != null)
        {
            // Randomly choose an edge for spawn
            Vector3 spawnPosition = Vector3.zero;
            int edge = Random.Range(0, 4); // 0 = North, 1 = South, 2 = East, 3 = West

            switch (edge)
            {
                case 0: // North edge
                    spawnPosition = new Vector3(Random.Range(-5, 5), 0, 30);
                    break;
                case 1: // South edge
                    spawnPosition = new Vector3(Random.Range(-5, 5), 0, -30);
                    break;
                case 2: // East edge
                    spawnPosition = new Vector3(30, 0, Random.Range(-5, 5));
                    break;
                case 3: // West edge
                    spawnPosition = new Vector3(-30, 0, Random.Range(-5, 5));
                    break;
            }

            // Instantiate the enemy prefab at the chosen position
            GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPosition, Quaternion.identity);
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.targetCPU = cpu; // Set the CPU as the enemy's target
        }
    }

    void SpawnBossEnemy()
    {
        if (cpu != null)
        {
            // Randomly choose an edge for spawn
            Vector3 spawnPosition = Vector3.zero;
            int edge = Random.Range(0, 4); // 0 = North, 1 = South, 2 = East, 3 = West

            switch (edge)
            {
                case 0: // North edge
                    spawnPosition = new Vector3(Random.Range(-5, 5), 0, 30);
                    break;
                case 1: // South edge
                    spawnPosition = new Vector3(Random.Range(-5, 5), 0, -30);
                    break;
                case 2: // East edge
                    spawnPosition = new Vector3(30, 0, Random.Range(-5, 5));
                    break;
                case 3: // West edge
                    spawnPosition = new Vector3(-30, 0, Random.Range(-5, 5));
                    break;
            }

            // Instantiate the enemy prefab at the chosen position
            GameObject enemy = Instantiate(bossEnemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPosition, Quaternion.identity);
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.targetCPU = cpu; // Set the CPU as the enemy's target
        }
    }
}
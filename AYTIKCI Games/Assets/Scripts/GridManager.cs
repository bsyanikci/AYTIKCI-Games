using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cpu; // The CPU at the center of the map
    public GameObject[] randomPrefabs; // The prefabs to be randomly placed
    public int gridSize = 10; // Size of the grid (10x10 for example)
    public float cellSize = 2.0f; // Size of each grid cell
    public int prefabPlacementProbability = 6;

    void Start()
    {
        GenerateRandomPrefabs();
    }

    void GenerateRandomPrefabs()
    {
        for (int x = -gridSize / 2; x < gridSize / 2; x++)
        {
            for (int z = -gridSize / 2; z < gridSize / 2; z++)
            {
                if (Random.Range(0, prefabPlacementProbability) ==1)
                {
                    Vector3 position = new Vector3(x * cellSize, 1.2f, z * cellSize);

                    // Avoid placing near the CPU
                    if (Vector3.Distance(position, cpu.transform.position) > 6)
                    {
                        int prefabIndex = Random.Range(0, randomPrefabs.Length);
                        Instantiate(randomPrefabs[prefabIndex], position, Quaternion.identity);
                    }
                }
                
            }
        }
    }
}

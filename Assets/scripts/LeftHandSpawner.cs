using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandSpawner : MonoBehaviour
{
    public GameObject[] prefabs; // Array to hold the three prefabs
    public float spawnSpeed = 5f; // Speed at which the prefabs are shot
    public float spawnInterval = 5f; // Time interval between spawns
    public int minSpawnCount = 1; // Minimum number of prefabs to shoot
    public int maxSpawnCount = 5; // Maximum number of prefabs to shoot

    private float nextSpawnTime;
    private bool canSpawn = false; // Flag to track if spawning is allowed


    void Start()
    {
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogError("No prefabs attached to LeftHandSpawner!");
        }
        nextSpawnTime = Time.time + spawnInterval; // Set the initial spawn time
    }

    void Update()
    {
        // If spawning is not allowed, skip the update
        if (!canSpawn) return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnPrefabs();
            nextSpawnTime = Time.time + spawnInterval; // Reset the spawn timer
        }
    }

    void SpawnPrefabs()
    {
        if (prefabs.Length == 0) return;

        // Randomize the number of prefabs to spawn (between minSpawnCount and maxSpawnCount)
        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            // Randomly select a prefab from the array
            int prefabIndex = Random.Range(0, prefabs.Length);
            GameObject prefabToSpawn = prefabs[prefabIndex];

            // Instantiate the prefab at the spawner's position
            GameObject spawnedObject = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

            // Apply forward velocity to the prefab
            Rigidbody spawnedObjectRB = spawnedObject.GetComponent<Rigidbody>();
            if (spawnedObjectRB != null)
            {
                spawnedObjectRB.velocity = transform.forward * spawnSpeed;
            }
            else
            {
                Debug.LogWarning($"Prefab {prefabToSpawn.name} does not have a Rigidbody!");
            }
        }
    }
    // Public method to enable spawning
    public void AllowSpawning()
    {
        canSpawn = true;
        Debug.Log("Left hand spawner is now enabled!");
    }
}

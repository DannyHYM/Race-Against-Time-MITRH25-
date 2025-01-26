using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject prefab1;             // First bubble prefab
    public GameObject prefab2;             // Second bubble prefab
    public float spawnInterval = 2f;       // Time between spawns
    public int bubblesPerTable = 3;        // Number of bubbles to spawn per table
    public float spawnHeight = 1f;         // Height above the table to spawn bubbles
    public int maxPrefab1 = 5;             // Maximum number of Prefab 1 bubbles
    public int maxPrefab2 = 10;            // Maximum number of Prefab 2 bubbles

    private int currentPrefab1Count = 0;   // Current count of Prefab 1 bubbles
    private int currentPrefab2Count = 0;   // Current count of Prefab 2 bubbles
    private float timer = 0f;              // Timer to track spawn interval
    private bool canSpawnBubbles = false;  // Flag to track if bubbles should spawn

    void Update()
    {
        if (!canSpawnBubbles) return; // Skip if not allowed to spawn

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnBubblesOnTables();
            timer = 0f;
        }
    }

    public void AllowBubbleSpawning()
    {
        canSpawnBubbles = true;
        Debug.Log("Bubble spawning is now enabled!");
    }

    void SpawnBubblesOnTables()
    {
        // Find all objects in the scene with "TABLE" in their parent name
        GameObject[] objectsInScene = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objectsInScene)
        {
            if (obj.transform.parent != null && obj.transform.parent.name.Contains("TABLE"))
            {
                SpawnBubblesAboveSurface(obj);
            }
        }
    }

    void SpawnBubblesAboveSurface(GameObject tableSurface)
    {
        for (int i = 0; i < bubblesPerTable; i++)
        {
            // Check if we can spawn more bubbles of either prefab
            if (currentPrefab1Count >= maxPrefab1 && currentPrefab2Count >= maxPrefab2)
            {
                Debug.Log("Maximum number of bubbles reached!");
                return;
            }

            // Randomly select a prefab based on availability
            GameObject selectedPrefab = null;
            if (currentPrefab1Count < maxPrefab1 && currentPrefab2Count < maxPrefab2)
            {
                selectedPrefab = Random.value < 0.5f ? prefab1 : prefab2;
            }
            else if (currentPrefab1Count < maxPrefab1)
            {
                selectedPrefab = prefab1;
            }
            else if (currentPrefab2Count < maxPrefab2)
            {
                selectedPrefab = prefab2;
            }

            // Increment the corresponding counter
            if (selectedPrefab == prefab1)
            {
                currentPrefab1Count++;
            }
            else if (selectedPrefab == prefab2)
            {
                currentPrefab2Count++;
            }

            // Random position above the table
            Vector3 spawnPosition = tableSurface.transform.position +
                                    new Vector3(Random.Range(-0.5f, 0.5f), spawnHeight, Random.Range(-0.5f, 0.5f));

            // Instantiate the selected prefab
            Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        }
    }
}

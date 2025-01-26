using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class Ghost : MonoBehaviour
{
    public float spawnTimer = 1f;                // Time interval for spawning
    public GameObject[] prefabsToSpawn;         // Array of prefabs to spawn
    public float minEdgeDistance = 0.3f;        // Minimum distance from edges of surfaces
    public MRUKAnchor.SceneLabels spawnLabels;  // Filter for surface labels
    public float normalOffset;                  // Offset along the surface normal

    private float timer;                        // Timer to track spawning intervals
    private bool isMRUKInitialized;             // Tracks if MRUK is initialized

    void Start()
    {
        timer = 0f; // Initialize the timer
        isMRUKInitialized = MRUK.Instance != null && MRUK.Instance.IsInitialized;

        // Check if at least one prefab is assigned
        if (prefabsToSpawn == null || prefabsToSpawn.Length == 0)
        {
            Debug.LogError("No prefabs assigned to spawn.");
        }
    }

    void Update()
    {
        if (!isMRUKInitialized || prefabsToSpawn == null || prefabsToSpawn.Length == 0)
            return;

        timer += Time.deltaTime;
        if (timer > spawnTimer)
        {
            SpawnGhost();
            timer -= spawnTimer;
        }
    }

    public void SpawnGhost()
    {
        // Ensure there are prefabs to spawn
        if (prefabsToSpawn == null || prefabsToSpawn.Length == 0)
        {
            Debug.LogError("No prefabs assigned for spawning.");
            return;
        }

        // Get the current room from MRUK
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();

        // Generate a random position on a valid surface
        if (room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.VERTICAL, minEdgeDistance, new LabelFilter(spawnLabels), out Vector3 pos, out Vector3 norm))
        {
            // Calculate the spawn position with normal offset
            Vector3 spawnPosition = pos + norm * normalOffset;

            // Randomly select one prefab from the array
            GameObject selectedPrefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];

            // Instantiate the selected prefab at the calculated position
            Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Failed to generate a valid position on the surface.");
        }
    }
}

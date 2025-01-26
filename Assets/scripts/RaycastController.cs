
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    public float rayDistance = 10f; // Distance the ray will travel
    public Color defaultRayColor = Color.white; // Default color of the ray
    public Color floorRayColor = Color.green; // Color of the ray when it detects the floor
    public Color waterRayColor = Color.blue; // Color of the ray when it detects water
    // public Color uiRayColor = Color.magenta;
    public GameObject treeContainerPrefab; // Prefab for the TreeContainer
    public LayerMask waterLayer; // Layer to detect water prefabs
    // public LayerMask uiLayer; // Add this field

    private LineRenderer lineRenderer; // For rendering the ray visually
    private bool rayEnabled = true; // Tracks whether the ray is enabled

    void Start()
    {
        // Add a LineRenderer component to visualize the ray
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.01f; // Adjust the width of the ray
        lineRenderer.endWidth = 0.01f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Basic material for the ray
        lineRenderer.startColor = defaultRayColor;
        lineRenderer.endColor = defaultRayColor;
    }

    void Update()
    {
        if (rayEnabled)
        {
            ShootRay();
        }
    }

    void ShootRay()
    {
        // Define the ray starting point and direction
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;

        // Cast the ray
        Ray ray = new Ray(rayOrigin, rayDirection);
        RaycastHit hit;

        // Enable the line renderer to visualize the ray
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, rayOrigin);
        lineRenderer.SetPosition(1, rayOrigin + rayDirection * rayDistance);

        // Check if the ray hits anything
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // Check if the object's parent name is "FLOOR"
            Transform parent = hit.collider.transform.parent;
            if (parent != null && parent.name == "FLOOR")
            {
                lineRenderer.startColor = floorRayColor;
                lineRenderer.endColor = floorRayColor;

                Debug.Log("Floor detected!");

                // If the ray is green and the trigger is pressed, spawn the tree
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    SpawnTree(hit.point);
                }
            }
            // Check if the hit object is on the water layer
            else if (((1 << hit.collider.gameObject.layer) & waterLayer) != 0)
            {
                lineRenderer.startColor = waterRayColor;
                lineRenderer.endColor = waterRayColor;

                Debug.Log("Water prefab detected!");
            }
            // Check if the hit object is on the UI layer
            // else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("UI"))
            // {
            //     lineRenderer.startColor = uiRayColor; // Turn pink
            //     lineRenderer.endColor = uiRayColor;

            //     Debug.Log("UI detected!");
            // }
            else
            {
                lineRenderer.startColor = defaultRayColor;
                lineRenderer.endColor = defaultRayColor;
            }

            // Adjust the end of the ray to the hit point
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            // Reset the ray color to white if nothing is hit
            lineRenderer.startColor = defaultRayColor;
            lineRenderer.endColor = defaultRayColor;
        }
    }

    void SpawnTree(Vector3 spawnPosition)
    {
        // Instantiate the TreeContainer prefab at the ray's hit position
        Instantiate(treeContainerPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("Tree spawned at: " + spawnPosition);

        // Notify the LeftHandSpawner to start spawning
        LeftHandSpawner leftHandSpawner = FindObjectOfType<LeftHandSpawner>();
        if (leftHandSpawner != null)
        {
            leftHandSpawner.AllowSpawning();
        }

        // Check if this is the first tree being planted
        BubbleSpawner bubbleSpawner = FindObjectOfType<BubbleSpawner>();
        if (bubbleSpawner != null)
        {
            bubbleSpawner.AllowBubbleSpawning();
        }

        // Disable the ray after spawning the tree
        //DisableRay();

        // Start the timer if it hasn't already started
        WaterRayController waterRayController = FindObjectOfType<WaterRayController>();
        if (waterRayController != null && !waterRayController.IsTimerRunning())
        {
            waterRayController.StartGameTimer();
        }
    }

    public void DisableRay()
    {
        rayEnabled = false;
        lineRenderer.enabled = false;
        Debug.Log("Raycast disabled after planting a tree!");
    }

    public void EnableRay()
    {
        rayEnabled = true;
        lineRenderer.enabled = true;
        Debug.Log("Raycast enabled for planting a new tree!");
    }
}

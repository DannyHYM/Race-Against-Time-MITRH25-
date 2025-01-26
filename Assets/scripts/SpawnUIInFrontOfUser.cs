using UnityEngine;

public class SpawnUIInFrontOfUser : MonoBehaviour
{
    public GameObject uiPrefab; // Reference to the UI prefab
    public float distanceFromCamera = 2.0f; // Distance to place the UI from the camera
    public float heightOffset = 0.5f; // Optional height adjustment

    private Transform cameraTransform;

    void Start()
    {
        // Find the camera rig
        cameraTransform = Camera.main?.transform;
        if (cameraTransform == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }

        // Spawn the UI in front of the camera
        Vector3 spawnPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
        spawnPosition.y += heightOffset; // Adjust height if needed
        Quaternion spawnRotation = Quaternion.LookRotation(cameraTransform.forward);

        GameObject spawnedUI = Instantiate(uiPrefab, spawnPosition, spawnRotation);

        // Optional: Parent the UI to the scene root to prevent unwanted movement
        spawnedUI.transform.SetParent(null); // Keeps the UI independent of the Camera Rig
    }
}

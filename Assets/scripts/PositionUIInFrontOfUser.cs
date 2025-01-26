using UnityEngine;

public class PositionUIInFrontOfUser : MonoBehaviour
{
    public float distanceFromUser = 2f; // Distance in front of the user
    public float heightOffset = 0.5f;   // Offset above the user's head

    private Transform userCamera; // Reference to the user's camera

    void Start()
    {
        // Find the user's camera (adjust for your setup)
        userCamera = Camera.main?.transform;

        if (userCamera == null)
        {
            Debug.LogError("User camera not found! Ensure the main camera is tagged correctly.");
            return;
        }

        PositionUI();
    }

    void PositionUI()
    {
        if (userCamera == null) return;

        // Calculate position in front of the user
        Vector3 positionInFront = userCamera.position + (userCamera.forward * distanceFromUser);
        positionInFront.y += heightOffset; // Adjust height if needed

        // Update the position and rotation of the UI
        transform.position = positionInFront;
        transform.rotation = Quaternion.LookRotation(transform.position - userCamera.position);
    }
}

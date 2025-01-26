using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // Cache the main camera reference
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Make the canvas face away from the camera
            transform.rotation = Quaternion.LookRotation(mainCamera.transform.position - transform.position);
        }
    }
}

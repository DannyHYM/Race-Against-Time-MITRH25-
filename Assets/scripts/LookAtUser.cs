using UnityEngine;

public class LookAtUser : MonoBehaviour
{
    public Transform userCamera; // Reference to the user's camera

    void Start()
    {
        if (userCamera == null)
        {
            userCamera = Camera.main.transform; // Automatically find the main camera if not assigned
        }
    }

    void Update()
    {
        if (userCamera != null)
        {
            // Make the UI face the user
            transform.LookAt(userCamera);
            
            // Rotate by 180 degrees to ensure it faces correctly
            transform.Rotate(0, 180, 0);
        }
    }
}

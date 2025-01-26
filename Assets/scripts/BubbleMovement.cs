using UnityEngine;

public class BubbleMovement : MonoBehaviour
{
    public float bubbleSpeed = 1f;        // Speed of bubble movement
    public float driftForce = 0.5f;      // Random drifting force
    public float directionChangeInterval = 2f; // Time between direction changes

    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody attached to the bubble prefab!");
            return;
        }

        // Apply an initial random velocity
        Vector3 initialVelocity = Random.insideUnitSphere * bubbleSpeed;
        rb.velocity = initialVelocity;

        // Start changing direction periodically
        InvokeRepeating(nameof(ChangeDirection), directionChangeInterval, directionChangeInterval);
    }

    void ChangeDirection()
    {
        if (rb == null) return;

        // Apply a random drifting force
        Vector3 randomForce = Random.insideUnitSphere.normalized * driftForce;
        rb.AddForce(randomForce, ForceMode.Force);
    }
}

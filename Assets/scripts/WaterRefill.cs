using UnityEngine;

public class WaterRefill : MonoBehaviour
{
    public float refillAmount = 5f; // Amount of water time to refill

    public void OnInteract()
    {
        // Logic for interacting with the bubble (e.g., popping or disappearing)
        Destroy(gameObject); // Destroy the bubble after interaction
    }
}

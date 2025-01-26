using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaterRayController : MonoBehaviour
{
    public GameObject waterEffectPrefab; // The visual water particle effect prefab
    public GameObject waterGunPrefab;    // The water gun prefab
    public float rayDistance = 10f;      // Maximum range of the ray
    public LayerMask treeLayer;          // Layer to detect tree containers
    public LayerMask interactableLayer;
    public TextMeshProUGUI treeCounterText; // Reference to the tree counter UI Text
    public TextMeshProUGUI timerText;       // Reference to the countdown timer UI Text
    public TextMeshProUGUI winMessage;       // Text component for "You Won!"
    public TextMeshProUGUI loseMessage;      // Text component for "You Lost!"       
    public int maxTrees = 5;             // Maximum number of trees to plant
    public float maxWaterTime = 5f;      // Maximum time (seconds) the user can spray
    public float gameDuration = 180f;    // Total game duration in seconds


    private int currentTreeCount = 0;
    private GameObject activeWaterEffect; // Reference to the active water spray effect
    private Transform rightHandAnchor;   // Reference to the right-hand anchor
    private bool isSpraying = false;     // Tracks if the spray trigger is held
    private float currentWaterTime;      // Current remaining time for spraying
    private float remainingGameTime;     // Remaining game time
    private bool timerRunning = false;   // Tracks if the game timer is running
    [SerializeField]
    private TMP_Text countdownText;          // Reference to the countdown text component
    private bool gameEnded = false;    // Tracks if the game has ended

    void Start()
    {
        // Initialize the water time
        currentWaterTime = maxWaterTime;
        remainingGameTime = gameDuration;

        // Find the RightHandAnchor at runtime
        rightHandAnchor = GameObject.Find("RightHandAnchor")?.transform;
        if (rightHandAnchor == null)
        {
            Debug.LogError("RightHandAnchor not found in the scene!");
            return;
        }

        // Instantiate the water gun and attach it to the right hand
        GameObject activeWaterGun = Instantiate(waterGunPrefab, rightHandAnchor.position, rightHandAnchor.rotation);
        activeWaterGun.transform.SetParent(rightHandAnchor); // Attach to the right-hand controller

        // Adjust local position and rotation to align correctly
        activeWaterGun.transform.localPosition = new Vector3(0, -0.05f, -0.11f); // Adjust as needed
        activeWaterGun.transform.localRotation = Quaternion.Euler(0, 0, 0);   // Adjust rotation for proper alignment

        // Find and assign the countdown text from the instantiated gun
        countdownText = activeWaterGun.GetComponentInChildren<TMP_Text>();
        if (countdownText == null)
        {
            Debug.LogError("Countdown Text not found in the water gun prefab!");
        }
        else
        {
            countdownText.text = "";
        }
        // Update the tree counter UI
        UpdateTreeCounterUI();
        UpdateTimerUI();

        // Hide both win and lose messages at the start
        if (winMessage != null) winMessage.gameObject.SetActive(false);
        if (loseMessage != null) loseMessage.gameObject.SetActive(false);

    }

    void Update()
    {
        // Handle game timer
        if (timerRunning)
        {
            remainingGameTime -= Time.deltaTime;

            if (remainingGameTime <= 0)
            {
                remainingGameTime = 0;
                timerRunning = false;
                EndGame(false);
            }

            UpdateTimerUI();
        }

        // Check if the spray trigger is pressed and if there is time remaining
        if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger) && currentWaterTime > 0)
        {
            if (!isSpraying)
            {
                StartSpraying();
            }

            // Decrease the timer while spraying
            currentWaterTime -= Time.deltaTime;

            // Update the countdown text
            UpdateCountdownText();

            // Shoot the invisible ray
            ShootWaterRay();

            // Pause spraying if the timer hits zero
            if (currentWaterTime <= 0)
            {
                StopSpraying();
            }
        }
        else
        {
            // Stop spraying if the trigger is released
            if (isSpraying)
            {
                StopSpraying();
            }
        }
        // Check for interaction with bubbles
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            InteractWithBubble();
        }
    }


    void StartSpraying()
    {
        isSpraying = true;

        if (activeWaterEffect != null)
        {
            // Access the ParticleSystem's emission module and enable it
            var particleSystem = activeWaterEffect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                var emission = particleSystem.emission;
                emission.enabled = true;
            }
        }
        else if (waterEffectPrefab != null && rightHandAnchor != null)
        {
            // Instantiate the water effect prefab if it doesn't exist
            activeWaterEffect = Instantiate(waterEffectPrefab, rightHandAnchor.position, rightHandAnchor.rotation);
            activeWaterEffect.transform.SetParent(rightHandAnchor); // Attach to the water gun
            activeWaterEffect.transform.localPosition = new Vector3(0, 0, 0.2f); // Adjust to the front of the gun
            activeWaterEffect.transform.localRotation = Quaternion.identity;     // Match the gun's rotation
        }

        Debug.Log("Spraying started!");
    }

    void StopSpraying()
    {
        isSpraying = false;

        // Notify the tree to stop watering
        Vector3 rayOrigin = rightHandAnchor.position;
        Vector3 rayDirection = rightHandAnchor.forward;

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance, treeLayer))
        {
            TreeGrowth treeGrowth = hit.collider.GetComponent<TreeGrowth>();
            if (treeGrowth != null)
            {
                treeGrowth.StopWatering(); // Notify the tree to stop watering
            }
        }

        // Disable the particle effect emission
        if (activeWaterEffect != null)
        {
            var particleSystem = activeWaterEffect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                var emission = particleSystem.emission;
                emission.enabled = false;
            }
        }

        Debug.Log("Spraying stopped!");
    }
    void InteractWithBubble()
    {
        // Define the ray's origin and direction
        Vector3 rayOrigin = rightHandAnchor.position;
        Vector3 rayDirection = rightHandAnchor.forward;

        // Cast the ray
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance, interactableLayer))
        {
            Debug.Log($"Interactable hit: {hit.collider.name}");

            // Check if the hit object has a WaterRefill script
            WaterRefill waterRefill = hit.collider.GetComponent<WaterRefill>();
            if (waterRefill != null)
            {
                // Replenish water and destroy the bubble
                ReplenishWater(waterRefill.refillAmount);
                waterRefill.OnInteract(); // Perform the bubble's interaction logic (e.g., pop)
            }
        }
    }

    void ShootWaterRay()
    {
        if (rightHandAnchor == null) return;

        // Define the ray's origin and direction
        Vector3 rayOrigin = rightHandAnchor.position;
        Vector3 rayDirection = rightHandAnchor.forward;

        // Cast the ray
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayDistance, treeLayer))
        {
            Debug.Log($"Water ray hit: {hit.collider.name}");

            // Check if the hit object has a TreeGrowth script
            TreeGrowth treeGrowth = hit.collider.GetComponent<TreeGrowth>();
            if (treeGrowth != null)
            {
                treeGrowth.StartWatering(); // Notify the tree that it's being watered
            }
        }
    }

    void UpdateCountdownText()
    {
        if (countdownText != null)
        {
            countdownText.text = "Current Water\n" + currentWaterTime.ToString("F1"); // Display one decimal place
        }
    }

    public void ReplenishWater(float amount)
    {
        currentWaterTime = Mathf.Clamp(currentWaterTime + amount, 0, maxWaterTime);
        UpdateCountdownText();
    }

    // Increment the tree counter and update the UI
    public void IncrementTreeCounter()
    {
        currentTreeCount++;
        UpdateTreeCounterUI();

        // Check if the goal is reached
        if (currentTreeCount >= maxTrees)
        {
            EndGame(true);
        }
    }

    // Update the tree counter UI
    void UpdateTreeCounterUI()
    {
        if (treeCounterText != null)
        {
            treeCounterText.text = $"Trees Planted: {currentTreeCount}/{maxTrees}";
        }
    }

    // Add this method near other timer logic
    public bool IsTimerRunning()
    {
        return timerRunning;
    }

    public void StartGameTimer()
    {
        if (!timerRunning)
        {
            timerRunning = true;
            Debug.Log("Game timer started!");
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            // int seconds = Mathf.CeilToInt(remainingGameTime); // Round up to ensure 0 is displayed correctly
            // timerText.text = $"Time Left: {seconds}s";

            timerText.text = $"Time Remaining: {remainingGameTime:F1}s";
        }
    }

    // End the game when the goal is reached
    void EndGame(bool won)
    {
        gameEnded = true;
        timerRunning = false;

        // Display the appropriate message
        if (won && winMessage != null)
        {
            winMessage.gameObject.SetActive(true);
            //winMessage.text = "You've Won!";
        }
        else if (!won && loseMessage != null)
        {
            loseMessage.gameObject.SetActive(true);
            //loseMessage.text = "You Lost!";
        }

        Debug.Log(won ? "Game Won!" : "Game Lost!");

        // Stop the left-hand spawner
        LeftHandSpawner leftHandSpawner = FindObjectOfType<LeftHandSpawner>();
        if (leftHandSpawner != null)
        {
            leftHandSpawner.enabled = false; // Disable the spawner script
        }
    }

}

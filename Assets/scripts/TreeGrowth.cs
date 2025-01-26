using UnityEngine;

public class TreeGrowth : MonoBehaviour
{
    public GameObject[] treeStages; // Array of tree stage prefabs (Stage 1, 2, 3)
    public float timeToGrow = 5f; // Duration required for the tree to grow to the next stage
    public UnityEngine.UI.Image progressBarBackground; // Reference to the progress bar's Background image
    public Color doneColor = Color.green; // Color for the progress bar when the tree is fully grown

    private RectTransform fillAreaRect; // Cached RectTransform for the Fill Area
    private int currentStage = 0; // Current growth stage (0 = Stage 1)
    private float cumulativeWaterTime = 0f; // Cumulative watering time
    private GameObject currentTree; // The current active tree stage prefab
    private bool isBeingWatered = false; // Tracks whether the tree is being watered
    private bool finalStageReached = false; // Tracks whether the final stage is reached

    private Vector2 initialAnchor = new Vector2(155, 15); // Starting coordinates for the Fill Area
    private Vector2 fullAnchor = new Vector2(4.3f, 164); // Full coordinates for the Fill Area

    void Start()
    {
        // Initialize tree stage and progress bar
        Transform fillAreaTransform = transform.Find("Canvas/Slider/Fill Area");
        if (fillAreaTransform != null)
        {
            fillAreaRect = fillAreaTransform.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("Fill Area not found in TreeContainer prefab!");
        }

        if (treeStages.Length > 0)
        {
            currentTree = Instantiate(treeStages[currentStage], transform.position, Quaternion.identity, transform);
        }

        ResetProgressBar();
    }

    void Update()
    {
        // Increment cumulative watering time only if isBeingWatered is true
        if (isBeingWatered)
        {
            cumulativeWaterTime += Time.deltaTime;

            if (fillAreaRect != null)
            {
                UpdateProgressBar(cumulativeWaterTime / timeToGrow);
            }

            if (cumulativeWaterTime >= timeToGrow)
            {
                GrowTree();
            }
        }
    }

    public void StartWatering()
    {
        isBeingWatered = true;
    }

    public void StopWatering()
    {
        isBeingWatered = false;
    }

    private void GrowTree()
    {
        // If the tree is at the final stage, mark it as done and stop growth
        if (currentStage >= treeStages.Length - 1)
        {
            // Prevent this block from running multiple times
            if (finalStageReached) return;
            finalStageReached = true;
            
            // Turn the progress bar background green to indicate the tree is done
            if (progressBarBackground != null)
            {
                progressBarBackground.color = doneColor;
            }

            Debug.Log("Tree is fully grown and stays in the scene!");


            // Notify the WaterRayController to increment the tree counter
            WaterRayController waterRayController = FindObjectOfType<WaterRayController>();
            if (waterRayController != null)
            {
                waterRayController.IncrementTreeCounter();
            }

            Debug.Log("Tree is fully grown and stays in the scene!");
            return;
        }

        // Reset the cumulative watering time
        cumulativeWaterTime = 0f;

        // Reset the Fill Area to its initial state
        if (fillAreaRect != null)
        {
            ResetProgressBar();
        }

        // Destroy the current tree
        Destroy(currentTree);

        // Advance to the next stage
        currentStage++;

        if (currentStage < treeStages.Length)
        {
            currentTree = Instantiate(treeStages[currentStage], transform.position, Quaternion.identity, transform);
            Debug.Log($"Tree grew to stage {currentStage + 1}");
        }
    }

    private void ResetProgressBar()
    {
        if (fillAreaRect != null)
        {
            fillAreaRect.offsetMin = new Vector2(initialAnchor.x, fillAreaRect.offsetMin.y);
            fillAreaRect.offsetMax = new Vector2(-initialAnchor.y, fillAreaRect.offsetMax.y);
        }
    }

    private void UpdateProgressBar(float progress)
    {
        progress = Mathf.Clamp01(progress);

        float currentLeft = Mathf.Lerp(initialAnchor.x, fullAnchor.x, progress);
        float currentRight = Mathf.Lerp(initialAnchor.y, fullAnchor.y, progress);

        fillAreaRect.offsetMin = new Vector2(currentLeft, fillAreaRect.offsetMin.y);
        fillAreaRect.offsetMax = new Vector2(-currentRight, fillAreaRect.offsetMax.y);
    }
}

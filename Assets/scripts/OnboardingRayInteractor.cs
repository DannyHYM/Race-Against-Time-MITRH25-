using UnityEngine;
using UnityEngine.EventSystems;

public class OnboardingRayInteractor : MonoBehaviour
{
    public float rayLength = 10f; // Length of the ray
    public LayerMask uiLayer; // Layer to detect UI elements
    private LineRenderer lineRenderer; // For rendering the ray visually
    public Color defaultRayColor = Color.white; // Default color of the ray



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
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;

        // Cast a ray from the controller
        Ray ray = new Ray(rayOrigin, rayDirection);
        RaycastHit hit;

        // Update the line renderer to visualize the ray
        lineRenderer.SetPosition(0, rayOrigin);
        lineRenderer.SetPosition(1, rayOrigin + rayDirection * rayLength);

        if (Physics.Raycast(ray, out hit, rayLength, uiLayer))
        {
            // Turn the ray green when hitting a UI element
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;

            // Check for interaction
            ExecuteUIInteraction(hit);
        }
        else
        {
            // Default to white if no UI is hit
            lineRenderer.startColor = defaultRayColor;
            lineRenderer.endColor = defaultRayColor;
        }
    }

    void ExecuteUIInteraction(RaycastHit hit)
    {
        // Trigger interaction with UI when the right index trigger is pressed
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) // Right index trigger
        {
            // Simulate a pointer click on the UI element
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(hit.collider.gameObject, pointerEventData, ExecuteEvents.pointerClickHandler);

            Debug.Log($"Clicked on: {hit.collider.name}");
        }
    }
}

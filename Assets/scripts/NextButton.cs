using UnityEngine;

public class NextButton : MonoBehaviour
{
    public OnboardingUIHandler onboardingUIHandler; // Reference to the OnboardingUIHandler script

    void Update()
    {
        // Check if the button is being clicked
        if (IsRayHovering() && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Debug.Log("Next button clicked!");

            if (onboardingUIHandler != null)
            {
                onboardingUIHandler.ShowNextUI();
            }
        }
    }

    private bool IsRayHovering()
    {
        // Use Unity's Physics system to detect ray hits on this button
        RaycastHit hit;
        Vector3 rayOrigin = Camera.main.transform.position; // Use the camera as the origin
        Vector3 rayDirection = Camera.main.transform.forward; // Cast forward from the camera

        if (Physics.Raycast(rayOrigin, rayDirection, out hit))
        {
            // Check if the ray hit this button
            return hit.collider != null && hit.collider.gameObject == this.gameObject;
        }

        return false;
    }
}

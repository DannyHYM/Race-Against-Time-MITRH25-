using UnityEngine;
using System.Collections.Generic;

public class OnboardingUIHandler : MonoBehaviour
{
    public Transform userCamera;          // Reference to the user's camera rig
    public List<GameObject> onboardingUIs; // List of onboarding UIs
    private int currentIndex = 0;         // Tracks the currently active onboarding UI

    void Start()
    {
        if (onboardingUIs == null || onboardingUIs.Count == 0)
        {
            Debug.LogError("No onboarding UIs assigned!");
            return;
        }

        PositionAllUIsInFrontOfUser();

        // Show only the first onboarding UI
        UpdateActiveUI();
    }

    void PositionAllUIsInFrontOfUser()
    {
        if (userCamera == null)
        {
            userCamera = Camera.main.transform;
        }

        foreach (var ui in onboardingUIs)
        {
            // Position each UI in front of the user
            Vector3 forwardPosition = userCamera.position + userCamera.forward * 1.5f; // 1.5m in front
            forwardPosition.y += 1.5f; // Height offset
            ui.transform.position = forwardPosition;

            // Make each UI face the user
            ui.transform.LookAt(userCamera);
            ui.transform.Rotate(0, 180, 0); // Correct for flipped orientation
        }
    }

    void UpdateActiveUI()
    {
        // Enable only the current onboarding UI and disable the rest
        for (int i = 0; i < onboardingUIs.Count; i++)
        {
            onboardingUIs[i].SetActive(i == currentIndex);
        }
    }

    public void ShowNextUI()
    {
        if (currentIndex < onboardingUIs.Count - 1)
        {
            currentIndex++;
            UpdateActiveUI();
        }
        else
        {
            EndOnboarding(); // End onboarding if no more UIs
        }
    }

    public void EndOnboarding()
    {
        // Hide all onboarding UIs
        foreach (var ui in onboardingUIs)
        {
            ui.SetActive(false);
        }

        Debug.Log("Onboarding Complete. Starting Game!");

        // TODO: Trigger the main game logic here
    }
}

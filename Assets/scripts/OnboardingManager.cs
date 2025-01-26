using UnityEngine;
using UnityEngine.Video;

public class OnboardingManager : MonoBehaviour
{
    public GameObject onboardingScreenPrefab; // Prefab with the screen and VideoPlayer
    public Transform userCamera;             // Reference to the user's camera rig
    private GameObject activeOnboardingScreen; // Tracks the instantiated onboarding screen
    private VideoPlayer videoPlayer;

    void Start()
    {
        if (onboardingScreenPrefab != null)
        {
            SpawnOnboardingScreen();
        }
        else
        {
            Debug.LogError("Onboarding Screen Prefab is not assigned!");
        }
    }

    void SpawnOnboardingScreen()
    {
        // Instantiate the onboarding screen
        activeOnboardingScreen = Instantiate(onboardingScreenPrefab);

        // Position it in front of the user
        PositionScreenInFrontOfUser(activeOnboardingScreen.transform);

        // Get the VideoPlayer component
        videoPlayer = activeOnboardingScreen.GetComponentInChildren<VideoPlayer>();
        if (videoPlayer != null)
        {
            videoPlayer.isLooping = true; // Ensure video loops
            videoPlayer.Play();
        }
        else
        {
            Debug.LogError("No VideoPlayer found on the onboarding screen prefab!");
        }
    }

    void PositionScreenInFrontOfUser(Transform screenTransform)
    {
        // Ensure the camera reference is set
        if (userCamera == null)
        {
            userCamera = Camera.main.transform;
        }

        // Position the screen
        Vector3 forwardPosition = userCamera.position + userCamera.forward * 2f; // 2m in front
        forwardPosition.y += 1.5f; // Adjust height offset
        screenTransform.position = forwardPosition;

        // Make the screen face the user
        screenTransform.LookAt(userCamera);
        screenTransform.Rotate(0, 180, 0); // Correct for flipped orientation
    }

    public void StopOnboarding()
    {
        // Stop the video and destroy the screen
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }

        if (activeOnboardingScreen != null)
        {
            Destroy(activeOnboardingScreen);
        }

        Debug.Log("Onboarding Complete. Starting Game!");
    }
}

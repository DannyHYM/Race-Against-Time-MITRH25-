using System.Collections; // Required for IEnumerator
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public string mainSceneName = "MR Scene"; // Replace with your main game scene name

    public void LoadMainScene()
    {
        // Load the game scene additively
        SceneManager.LoadScene(mainSceneName, LoadSceneMode.Additive);

        // Unload the onboarding scene
        StartCoroutine(UnloadOnboardingScene());
    }

    private IEnumerator UnloadOnboardingScene()
    {
        yield return new WaitForSeconds(1f); // Optional delay
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        Debug.Log("Onboarding Scene Unloaded");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSprayController : MonoBehaviour
{
    public GameObject waterEffectPrefab; // The Particle System prefab
    private GameObject activeWaterEffect; // Reference to the active particle system
    private ParticleSystem waterParticleSystem; // Reference to the Particle System component

    void Update()
    {
        // Check if the right-hand grip trigger (inner trigger) is pressed
        if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)) // Right middle finger trigger
        {
            if (activeWaterEffect == null)
            {
                StartWatering();
            }
        }
        else
        {
            if (activeWaterEffect != null)
            {
                StopWatering();
            }
        }
    }

    void StartWatering()
    {
        if (waterEffectPrefab == null)
        {
            Debug.LogError("Water Effect Prefab is not assigned!");
            return;
        }

        // Instantiate the water effect prefab
        activeWaterEffect = Instantiate(waterEffectPrefab, transform.position, Quaternion.identity);
        activeWaterEffect.transform.SetParent(transform); // Attach to the controller
        activeWaterEffect.transform.localPosition = Vector3.zero; // Align with the controller
        activeWaterEffect.transform.localRotation = Quaternion.identity; // Match the controller's rotation

        // Get the ParticleSystem component
        waterParticleSystem = activeWaterEffect.GetComponent<ParticleSystem>();
        if (waterParticleSystem != null)
        {
            waterParticleSystem.Play(); // Start the particle system
        }
        else
        {
            Debug.LogError("No ParticleSystem component found on the Water Effect Prefab!");
        }

        Debug.Log("Watering started!");
    }

    void StopWatering()
    {
        if (activeWaterEffect != null && waterParticleSystem != null)
        {
            // Stop emitting new particles but allow existing ones to finish
            var emission = waterParticleSystem.emission;
            emission.enabled = false;

            // Destroy the GameObject after all particles have finished
            Destroy(activeWaterEffect, waterParticleSystem.main.startLifetime.constantMax);
            activeWaterEffect = null;

            Debug.Log("Watering stopped!");
        }
    }
}

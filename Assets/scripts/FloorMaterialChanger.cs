using UnityEngine;
using Meta.XR.MRUtilityKit;

public class FloorMaterialChanger : MonoBehaviour
{
    [SerializeField]
    private Material grassMaterial;

    [SerializeField]
    private EffectMesh floorEffectMesh;

    private void Start()
    {
        if (MRUK.Instance != null)
        {
            MRUK.Instance.RegisterSceneLoadedCallback(OnSceneLoaded);
        }
    }

    private void OnSceneLoaded()
    {
        // Configure floor EffectMesh
        if (floorEffectMesh != null)
        {
            // Set to only handle floor elements
            floorEffectMesh.Labels = MRUKAnchor.SceneLabels.FLOOR;
            
            // Apply grass material
            floorEffectMesh.MeshMaterial = grassMaterial;
            
            // Create the mesh for the current room
            floorEffectMesh.CreateMesh(MRUK.Instance.GetCurrentRoom());
        }
    }
}
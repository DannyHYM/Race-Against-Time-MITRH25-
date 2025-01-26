using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    private TreeGrowth treeGrowth;

    void Start()
    {
        // Find the TreeGrowth script on the parent object
        treeGrowth = GetComponentInParent<TreeGrowth>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (treeGrowth != null)
        {
            treeGrowth.StartWatering();
        }
    }

    void OnParticleExit(Collider other)
    {
        if (treeGrowth != null)
        {
            treeGrowth.StopWatering();
        }
    }
}

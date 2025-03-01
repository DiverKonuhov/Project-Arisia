using UnityEngine;

public class OptimizationManager : MonoBehaviour
{
    public void EnableGPUInstancing(Material material)
    {
        material.enableInstancing = true;
    }

    public void SetupOcclusionCulling()
    {
        // Включение Occlusion Culling через настройки Unity
        Debug.Log("Occlusion Culling включен через настройки Unity.");
    }
}
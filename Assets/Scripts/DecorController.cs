// DecorController.cs
using UnityEngine;
using System.Collections;

public class DecorController : MonoBehaviour
{
    [SerializeField] private GameObject decorInstance;
    [SerializeField] private Renderer debugSphere;

    private TerrainDecorSystem system;
    private Transform mainCamera;
    private Vector3 initialDecorPosition;
    private bool isVisible;

    public void Initialize(TerrainDecorSystem system)
    {
        this.system = system;
        
        // Create decor
        if (system.decorPrefab)
        {
            decorInstance = Instantiate(system.decorPrefab, transform);
            decorInstance.SetActive(false);
            initialDecorPosition = decorInstance.transform.localPosition - Vector3.up * 2f;
            decorInstance.transform.localPosition = initialDecorPosition;
        }

        // Create debug sphere
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.SetParent(transform);
        sphere.transform.localPosition = Vector3.zero;
        sphere.transform.localScale = Vector3.one * 0.3f;
        debugSphere = sphere.GetComponent<Renderer>();
        debugSphere.enabled = system.showDebugSpheres;
        
        // Proper destruction method
        if (Application.isPlaying) Destroy(sphere.GetComponent<Collider>());
        else DestroyImmediate(sphere.GetComponent<Collider>());

        if (Application.isPlaying) StartCoroutine(VisibilityCheck());
    }

    IEnumerator VisibilityCheck()
    {
        while (true)
        {
            if (!mainCamera) mainCamera = Camera.main.transform;
            if (mainCamera) UpdateVisibility();
            
            yield return new WaitForSeconds(0.2f);
        }
    }

    void UpdateVisibility()
    {
        if (!decorInstance) return;

        Vector3 toDecor = transform.position - mainCamera.position;
        float distance = toDecor.magnitude;
        float angle = Vector3.Angle(mainCamera.forward, toDecor.normalized);

        bool shouldShow = angle < system.viewAngle/2 && 
                        distance < system.viewDistance && 
                        distance > 0.5f;

        if (shouldShow != isVisible)
        {
            isVisible = shouldShow;
            debugSphere.enabled = system.showDebugSpheres && !isVisible;
            
            StopAllCoroutines();
            StartCoroutine(AnimateDecor(isVisible, distance));
        }
    }

    IEnumerator AnimateDecor(bool show, float currentDistance)
    {
        Vector3 startPos = decorInstance.transform.localPosition;
        Vector3 targetPos = show ? Vector3.zero : initialDecorPosition;
        
        bool useAnimation = currentDistance > system.animationStartDistance;
        float duration = useAnimation ? system.appearDuration : 0f;

        decorInstance.SetActive(true);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = system.appearCurve.Evaluate(elapsed / duration);
            decorInstance.transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        decorInstance.transform.localPosition = targetPos;
        decorInstance.SetActive(show);
    }
}
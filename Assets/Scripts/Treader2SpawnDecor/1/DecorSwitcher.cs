using UnityEngine;
using System.Collections;

public class DecorSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject decor;
    [SerializeField] private Renderer sphereRenderer;
    [SerializeField] private float updateInterval = 0.2f;

    private Camera mainCamera;
    private float viewAngle;
    private float checkDistance;

    public void Initialize(GameObject prefab, bool showSphere, float angle, float distance, float heightOffset)
    {
        // Create sphere
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.SetParent(transform);
        sphere.transform.localPosition = Vector3.zero;
        sphere.transform.localScale = Vector3.one * 0.3f;
        sphereRenderer = sphere.GetComponent<Renderer>();
        DestroyImmediate(sphere.GetComponent<Collider>());

        // Adjust height
        Vector3 pos = transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(pos) + heightOffset;
        transform.position = pos;

        // Create decor
        if (prefab)
        {
            decor = Instantiate(prefab, transform);
            decor.transform.localPosition = Vector3.zero;
            decor.SetActive(false);
        }

        viewAngle = angle;
        checkDistance = distance;
        StartCoroutine(VisibilityCheck());
    }

    IEnumerator VisibilityCheck()
    {
        while (true)
        {
            if (!mainCamera) mainCamera = Camera.main;
            if (mainCamera) UpdateVisibility();
            yield return new WaitForSeconds(updateInterval);
        }
    }

    void UpdateVisibility()
    {
        Vector3 toDecor = transform.position - mainCamera.transform.position;
        float distance = toDecor.magnitude;
        float angle = Vector3.Angle(mainCamera.transform.forward, toDecor.normalized);

        bool shouldShowDecor = angle < viewAngle/2 && 
                             distance < checkDistance && 
                             IsInCameraView();

        sphereRenderer.enabled = !shouldShowDecor;
        if (decor) decor.SetActive(shouldShowDecor);
    }

    bool IsInCameraView()
    {
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);
        return viewportPos.z > 0 && 
               viewportPos.x >= 0 && viewportPos.x <= 1 &&
               viewportPos.y >= 0 && viewportPos.y <= 1;
    }
}
using UnityEngine;
using System.Collections;

public class DecorController : MonoBehaviour
{
    private TerrainDecorSystem system;
    private Transform mainCamera;
    private GameObject decorInstance;
    private GameObject debugSphere;

    public void Initialize(TerrainDecorSystem system, GameObject prefab)
    {
        this.system = system;
        CreateVisuals(prefab);
        StartCoroutine(VisibilityCheck());
    }

    void CreateVisuals(GameObject prefab)
    {
        // Создаем префаб декора
        decorInstance = Instantiate(prefab, transform.position, Quaternion.identity, transform);
        decorInstance.SetActive(false);

        // Создаем дебаг сферу только в редакторе
        #if UNITY_EDITOR
        debugSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        debugSphere.transform.SetParent(transform);
        debugSphere.transform.localPosition = Vector3.zero;
        debugSphere.transform.localScale = Vector3.one * 0.5f;
        debugSphere.GetComponent<Renderer>().material.color = Color.red;
        debugSphere.SetActive(!Application.isPlaying);
        #endif
    }

    IEnumerator VisibilityCheck()
    {
        while (true)
        {
            if (!mainCamera) mainCamera = Camera.main.transform;
            if (mainCamera) UpdateVisibility();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void UpdateVisibility()
    {
        if (!decorInstance) return;

        Vector3 toDecor = transform.position - mainCamera.position;
        float distance = toDecor.magnitude;
        float angle = Vector3.Angle(mainCamera.forward, toDecor.normalized);

        bool shouldShow = angle < system.viewAngle / 2 && 
                        distance < system.viewDistance;

        decorInstance.SetActive(shouldShow);

        #if UNITY_EDITOR
        if (debugSphere) debugSphere.SetActive(!shouldShow && !Application.isPlaying);
        #endif
    }
}
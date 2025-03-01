using UnityEngine;

public class LODManager : MonoBehaviour
{
    [Header("Настройки LOD")]
    public GameObject[] lodObjects; // Список объектов LOD
    public float[] lodDistances; // Расстояния для переключения LOD

    private void Update()
    {
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);

        for (int i = 0; i < lodObjects.Length; i++)
        {
            lodObjects[i].SetActive(distance <= lodDistances[i]);
        }
    }
}
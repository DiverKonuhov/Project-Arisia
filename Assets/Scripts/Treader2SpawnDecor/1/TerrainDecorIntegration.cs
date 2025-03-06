using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TerrainDecorIntegration : MonoBehaviour
{
    [Header("Terrain Settings")]
    public Terrain targetTerrain;
    public int treePrototypeIndex = 0;
    public float heightOffset = 0.5f;

    [Header("Visual Settings")]
    public GameObject decorPrefab;
    public bool alwaysShowSphere = true;
    [Range(1, 180)] public float viewAngle = 60f;
    public float checkDistance = 50f;

    [Header("Advanced")]
    public string treeNameFilter = "Decor";
    public bool liveUpdate = true;

    private List<GameObject> decorSpheres = new List<GameObject>();
    private TerrainData terrainData;

    void OnEnable() => RefreshSystem();
    
    void Update()
    {
        if (liveUpdate && !Application.isPlaying)
        {
            if (targetTerrain.terrainData.treeInstanceCount != decorSpheres.Count)
                RefreshSystem();
        }
    }

    void RefreshSystem()
    {
        if (!targetTerrain) targetTerrain = Terrain.activeTerrain;
        if (!targetTerrain) return;

        terrainData = targetTerrain.terrainData;
        ClearSpheres();
        CreateSpheresFromTrees();
    }

    void CreateSpheresFromTrees()
    {
        foreach (TreeInstance tree in terrainData.treeInstances)
        {
            if (tree.prototypeIndex != treePrototypeIndex) continue;
            
            var treePrefab = terrainData.treePrototypes[tree.prototypeIndex].prefab;
            if (!treePrefab.name.Contains(treeNameFilter)) continue;

            Vector3 worldPos = ConvertToWorldPosition(tree.position);
            CreateDecorSphere(worldPos);
        }
    }

    Vector3 ConvertToWorldPosition(Vector3 terrainPos)
    {
        return new Vector3(
            terrainPos.x * terrainData.size.x,
            terrainPos.y * terrainData.size.y,
            terrainPos.z * terrainData.size.z
        ) + targetTerrain.transform.position;
    }

    void CreateDecorSphere(Vector3 position)
    {
        GameObject sphere = new GameObject("DecorSphere");
        sphere.transform.position = position;
        sphere.transform.SetParent(targetTerrain.transform);

        var switcher = sphere.AddComponent<DecorSwitcher>();
        switcher.Initialize(decorPrefab, alwaysShowSphere, viewAngle, checkDistance, heightOffset);

        decorSpheres.Add(sphere);
    }

    void ClearSpheres()
    {
        foreach (GameObject sphere in decorSpheres)
        {
            if (sphere) DestroyImmediate(sphere);
        }
        decorSpheres.Clear();
    }

    #if UNITY_EDITOR
    void OnValidate() => RefreshSystem();
    #endif
}
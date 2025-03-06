// TerrainDecorSystem.cs
using UnityEngine;
using System.Collections.Generic;

public class TerrainDecorSystem : MonoBehaviour
{
    [Header("Main Settings")]
    public Terrain targetTerrain;
    [Tooltip("Index from Terrain's tree prototypes list")]
    public int treePrototypeIndex = 0;
    public GameObject decorPrefab;

    [Header("Visibility Settings")]
    public bool showDebugSpheres = true;
    [Range(1, 180)] public float viewAngle = 60f;
    public float viewDistance = 30f;

    [Header("Spawning Settings")]
    [Range(0.1f, 1f)] public float spawnDensity = 0.8f;
    public float minDistanceBetween = 2f;

    [Header("Animation Settings")]
    public float animationStartDistance = 15f;
    public float appearDuration = 1f;
    public AnimationCurve appearCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private List<DecorController> controllers = new List<DecorController>();
    private HashSet<Vector3> spawnedPositions = new HashSet<Vector3>();

    void Start() => InitializeSystem();

    void InitializeSystem()
    {
        ClearSystem();
        if (!targetTerrain) targetTerrain = Terrain.activeTerrain;
        if (targetTerrain) SpawnDecorObjects();
    }

    void SpawnDecorObjects()
    {
        foreach (TreeInstance tree in targetTerrain.terrainData.treeInstances)
        {
            if (tree.prototypeIndex != treePrototypeIndex) continue;
            if (Random.value > spawnDensity) continue;

            Vector3 worldPos = ConvertTreePosition(tree.position);
            if (IsPositionValid(worldPos))
            {
                CreateController(worldPos);
                spawnedPositions.Add(worldPos);
            }
        }
    }

    bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 pos in spawnedPositions)
        {
            if (Vector3.Distance(pos, position) < minDistanceBetween)
                return false;
        }
        return true;
    }

    Vector3 ConvertTreePosition(Vector3 terrainPos)
    {
        Vector3 size = targetTerrain.terrainData.size;
        return targetTerrain.transform.position + 
            new Vector3(
                terrainPos.x * size.x,
                terrainPos.y * size.y,
                terrainPos.z * size.z
            );
    }

    void CreateController(Vector3 position)
    {
        GameObject obj = new GameObject("DecorController");
        obj.transform.position = position;
        obj.transform.SetParent(transform);

        var controller = obj.AddComponent<DecorController>();
        controller.Initialize(this);
        controllers.Add(controller);
    }

    void ClearSystem()
    {
        foreach (DecorController c in controllers)
        {
            if (c) 
            {
                if (Application.isPlaying) Destroy(c.gameObject);
                else DestroyImmediate(c.gameObject);
            }
        }
        controllers.Clear();
        spawnedPositions.Clear();
    }

    #if UNITY_EDITOR
    void OnValidate()
    {
        if (Application.isPlaying) return;
        InitializeSystem();
    }
    #endif
}
using UnityEngine;
using System.Collections.Generic;

public class TerrainDecorSystem : MonoBehaviour
{
    [Header("Main Settings")]
    public Terrain targetTerrain;
    public int treePrototypeIndex = 0;
    public GameObject decorPrefab;

    [Header("View Settings")]
    [Range(1, 180)] public float viewAngle = 60f;
    public float viewDistance = 30f;

    private List<DecorController> controllers = new List<DecorController>();

    void Start()
    {
        ClearSystem();
        if (!targetTerrain) targetTerrain = Terrain.activeTerrain;
        if (targetTerrain && decorPrefab) SpawnDecor();
    }

    void SpawnDecor()
    {
        foreach (TreeInstance tree in targetTerrain.terrainData.treeInstances)
        {
            if (tree.prototypeIndex != treePrototypeIndex) continue;
            
            Vector3 worldPos = ConvertTreePosition(tree.position);
            CreateController(worldPos);
        }
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
        controller.Initialize(this, decorPrefab);
        controllers.Add(controller);
    }

    void ClearSystem()
    {
        foreach (DecorController c in controllers)
            if (c) Destroy(c.gameObject);
        controllers.Clear();
    }
}
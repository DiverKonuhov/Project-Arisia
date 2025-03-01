using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public Terrain terrain;

    public Vector3 GetRandomPositionOnTerrain()
    {
        float x = Random.Range(0, terrain.terrainData.size.x);
        float z = Random.Range(0, terrain.terrainData.size.z);
        float y = terrain.SampleHeight(new Vector3(x, 0, z));

        return new Vector3(x, y, z);
    }

    public void RemoveUnnecessaryAreas(Vector3 position, float radius)
    {
        // Удаляет участки Terrain с помощью Paint Holes
        TerrainData terrainData = terrain.terrainData;
        int resolution = terrainData.holesResolution;
        int holeX = Mathf.FloorToInt((position.x / terrainData.size.x) * resolution);
        int holeZ = Mathf.FloorToInt((position.z / terrainData.size.z) * resolution);

        bool[,] holes = terrainData.GetHoles(0, 0, resolution, resolution);
        for (int x = -Mathf.FloorToInt(radius); x <= Mathf.FloorToInt(radius); x++)
        {
            for (int z = -Mathf.FloorToInt(radius); z <= Mathf.FloorToInt(radius); z++)
            {
                holes[holeX + x, holeZ + z] = true;
            }
        }

        terrainData.SetHoles(0, 0, holes);
    }
}
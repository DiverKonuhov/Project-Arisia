using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class TreeManager : MonoBehaviour
{
    [Inject] private ObjectPoolManager _objectPoolManager;
    [Inject] private TerrainManager _terrainManager;

    [Header("Настройки деревьев")]
    public List<GameObject> treePrefabs; // Префабы деревьев
    public int treeCount = 100; // Количество деревьев для размещения

    private List<GameObject> _activeTrees = new List<GameObject>();

    private void Start()
    {
        // Размещение деревьев на Terrain
        SpawnTrees();
    }

    private void SpawnTrees()
    {
        for (int i = 0; i < treeCount; i++)
        {
            Vector3 randomPosition = _terrainManager.GetRandomPositionOnTerrain();
            GameObject tree = _objectPoolManager.GetObjectFromPool(treePrefabs[Random.Range(0, treePrefabs.Count)], randomPosition, Quaternion.identity);
            _activeTrees.Add(tree);
        }
    }

    public void ReplaceTree(GameObject oldTree, GameObject newTreePrefab)
    {
        Vector3 position = oldTree.transform.position;
        Quaternion rotation = oldTree.transform.rotation;

        _objectPoolManager.ReturnObjectToPool(oldTree);
        GameObject newTree = _objectPoolManager.GetObjectFromPool(newTreePrefab, position, rotation);
        _activeTrees.Add(newTree);
    }

    public void RemoveTree(GameObject tree)
    {
        _objectPoolManager.ReturnObjectToPool(tree);
        _activeTrees.Remove(tree);
    }
}
using UnityEngine;
using Lean.Pool;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject defaultPrefab;
    private Dictionary<GameObject, List<GameObject>> poolDictionary = new Dictionary<GameObject, List<GameObject>>();

    public GameObject GetObjectFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject obj = LeanPool.Spawn(prefab, position, rotation);
        return obj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        LeanPool.Despawn(obj);
    }
}
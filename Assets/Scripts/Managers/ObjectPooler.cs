using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    private Dictionary<GameObject, List<GameObject>> pools = new Dictionary<GameObject, List<GameObject>>();

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(prefab))
        {
            pools[prefab] = new List<GameObject>();
        }

        foreach (GameObject obj in pools[prefab])
        {
            if (!obj.activeInHierarchy) 
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true); 
                return obj;
            }
        }

        GameObject newObj = Instantiate(prefab, position, rotation);
        pools[prefab].Add(newObj);
        return newObj;
    }
}
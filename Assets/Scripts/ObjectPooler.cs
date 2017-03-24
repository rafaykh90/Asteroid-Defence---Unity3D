using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This is used to create pool for asteroid. But this can also be used to create a pool for any gameobject by dragging a new
/// prefab in the hierarchy and assigning that Gameobject to the pooledObject in the inspector.
/// </summary>
public class ObjectPooler : MonoBehaviour {

    public static ObjectPooler current;
    public GameObject pooledObject;
    public int pooledAmount = 10;
    public bool willGrow = true;
    public List<GameObject> pooledObjects;


    void Awake()
    {
        current = this;
    }

	void Start () {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)GameObject.Instantiate(pooledObject);
            pooledObjects.Add(obj);
        }
	}
	
    /// <summary>
    /// Returns a Gameobject from the pool which is inactive in the hierarchy. This method is called when a Gameobject from the
    /// pool is required.
    /// </summary>
    public GameObject getPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (willGrow)
        {
            GameObject obj = GameObject.Instantiate(pooledObject) as GameObject;
            pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }
}

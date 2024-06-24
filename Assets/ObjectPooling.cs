using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public GameObject objectToPoll;
    public int amountToPoll = 20;
    public List<GameObject> pooledObjects;
    public bool willGrow = true;

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPoll; i++)
        {
            GameObject obj = Instantiate(objectToPoll, transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
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
            GameObject obj = Instantiate(objectToPoll, transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }
}

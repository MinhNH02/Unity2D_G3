using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] GameObject poolPrefab;
    public Dictionary<int, ObjectPool> poolList;
    private void Awake()
    {
        poolList = new Dictionary<int, ObjectPool>();

    }
    public void CreatePool(PoolObjectData newPoolData)
    {
        GameObject newObjectPoolGO = Instantiate(poolPrefab, transform).gameObject;

        ObjectPool newObjectPool = newObjectPoolGO.GetComponent<ObjectPool>();
        newObjectPool.Set(newPoolData);
        newObjectPoolGO.name = "Pool: " + newPoolData.name;

        poolList.Add(newPoolData.poolId, newObjectPool);
    }

    public GameObject GetObject(PoolObjectData poolObjectData)
    {
        if (poolList.ContainsKey(poolObjectData.poolId) == false)
        {
            CreatePool(poolObjectData);
        }
        return poolList[poolObjectData.poolId].GetObject();
    }
    public bool checkWinning(Dictionary<int, ObjectPool> poolList)
    {
        bool isWin = false;
        foreach (KeyValuePair<int, ObjectPool> pair in poolList)
        {
            if (pair.Value != null)
            {
                if (pair.Value.pool.Count == 0)
                {
                    return true;
                }
            }
        }
        return isWin;
    }
}

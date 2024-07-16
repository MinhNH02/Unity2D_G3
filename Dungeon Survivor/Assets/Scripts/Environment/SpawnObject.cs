using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] GameObject spawnObject;
    [SerializeField] [Range(0,1)] float probability;

    public void Spawn()
    {
        if(Random.value < probability)
        {
            SpawnManager.instance.SpawnObject(spawnObject);
        }
    }
}

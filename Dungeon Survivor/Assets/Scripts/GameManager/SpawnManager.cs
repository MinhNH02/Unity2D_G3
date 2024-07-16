using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameObject player;
    public static SpawnManager instance;
    [SerializeField] private Vector2 spawnArea;

    private void Awake()
    {
        player = GameManager.instance.playerTranform.gameObject;
        instance = this;
    }
    public void SpawnObject(GameObject toSpawn)
    {

        /*float yRandom = Random.Range(-spawnArea.y, spawnArea.y);
        float xRandom = Random.Range(-spawnArea.x, spawnArea.x);
        if (xRandom != spawnArea.x || xRandom != -spawnArea.x)
        {
            int rdNum = Random.Range(0, 1);
            yRandom = rdNum == 1 ? spawnArea.y : -spawnArea.y;
        }*/

        //Vector3 positionToSpawn = new Vector3(xRandom, yRandom, 0);
        Vector3 positionToSpawn = new Vector3();

        float f = UnityEngine.Random.value > 0.5f ? -1f : 1f;
        if(UnityEngine.Random.value > 0.5f)
        {
            positionToSpawn.x = UnityEngine.Random.Range(-spawnArea.x, spawnArea.x);
            positionToSpawn.y = spawnArea.y * f;
        }
        else
        {
            positionToSpawn.x = spawnArea.x * f;
            positionToSpawn.y = UnityEngine.Random.Range(-spawnArea.y, spawnArea.y);
        }
        positionToSpawn.z = 0;

        positionToSpawn += player.transform.position;

        Transform dropItem = Instantiate(toSpawn, transform).transform;
        dropItem.transform.position = positionToSpawn;
    }
}

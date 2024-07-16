using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityTools : MonoBehaviour
{
    public static Vector3 GenerateRandomPosition(Vector2 spawnArea)
    {
        Vector3 position = new Vector3();

        float yRandom = Random.Range(-spawnArea.y, spawnArea.y);
        float xRandom = Random.Range(-spawnArea.x, spawnArea.x);
        if (xRandom != spawnArea.x || xRandom != -spawnArea.x)
        {
            int rdNum = Random.Range(0, 1);
            yRandom = rdNum == 1 ? spawnArea.y : -spawnArea.y;
        }

        position.z = 0;

        return position;
    }
}

using UnityEngine;

public class SpawnEnemyController : MonoBehaviour
{
    public float spawnAmount = 6f;
    public float spawnRate = 2f;
    ObjectPooling enemyPools;
    private float time = 0;
    //public RectTransform enemyHealthbarTransform;

    private void Start()
    {
        enemyPools = GetComponent<ObjectPooling>();
    }

    private void Update()
    {
        if (ActivedChildCount() < spawnAmount)
        {
            time += Time.deltaTime;
            if (time >= spawnRate)
            {
                time = 0;
                Spawn();
            }
        }
    }

    private int ActivedChildCount()
    {
        int count = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf) count++;
        }
        return count;
    }

    private void Spawn()
    {
        var enemy = enemyPools.GetPooledObject();
        enemy.SetActive(true);
    }
}

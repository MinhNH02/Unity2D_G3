using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesSpawnGroup
{
    public EnemyData EnemyData;
    public int count;
    public bool isBoss;

    public float repeatTimer;
    public float timeBetweenSpawn;
    public int repeatCount;
    public EnemiesSpawnGroup(EnemyData enemyData, int count, bool isBoss)
    {
        EnemyData = enemyData;
        this.count = count;
        this.isBoss = isBoss;
    }
    public void SetRepeatSpawn(float timeBetweenSpawns, int repeatCount)
    {
        this.timeBetweenSpawn = timeBetweenSpawns;
        this.repeatCount = repeatCount;
        repeatTimer = timeBetweenSpawn;
    }
}
public class EnemyManager : MonoBehaviour
{
    private GameObject player;
    [SerializeField] StageProgress stageProgress;
    [SerializeField] public PoolManager poolManager;
    [SerializeField] Vector2 spawnArea;

    List<Enemy> bossEnemiesList;
    int totalBossHealth;
    int currentBossHealth;
    [SerializeField] Slider bossHealthBar;

    List<EnemiesSpawnGroup> enemiesSpawnGroupList;
    List<EnemiesSpawnGroup> repeatedSpawnGroupList;

    int spawnPerFrame = 2;
    private void Start()
    {
        player = GameManager.instance.playerTranform.gameObject;
        bossHealthBar = FindObjectOfType<BossHPBar>(true).GetComponent<Slider>();
        stageProgress = FindObjectOfType<StageProgress>();
    }
    private void Update()
    {
        ProcessSpawn();
        ProcessRepeatedSpawnGroups();
        UpdateBossHealth();
    }

    private void ProcessRepeatedSpawnGroups()
    {
        if (repeatedSpawnGroupList == null) { return; }
        int count = repeatedSpawnGroupList.Count;
        List<EnemiesSpawnGroup> list = repeatedSpawnGroupList;
        for (int i = count - 1; i >= 0; i--)
        {
            repeatedSpawnGroupList[i].repeatTimer -= Time.deltaTime * 0.5f;
            Debug.Log(repeatedSpawnGroupList[i].repeatTimer);
            if (repeatedSpawnGroupList[i].repeatTimer < 0)
            {
                repeatedSpawnGroupList[i].repeatTimer = repeatedSpawnGroupList[i].timeBetweenSpawn;
                AddGroupToSpawn(repeatedSpawnGroupList[i].EnemyData, repeatedSpawnGroupList[i].count, repeatedSpawnGroupList[i].isBoss);
                repeatedSpawnGroupList[i].repeatCount -= 1;
                if (repeatedSpawnGroupList[i].repeatCount <= 0)
                {
                    repeatedSpawnGroupList.RemoveAt(i);
                }
                if (i == 0)
                {
                    repeatedSpawnGroupList = list;
                    count = repeatedSpawnGroupList.Count;
                }
            }
        }
    }

    private void ProcessSpawn()
    {
        if (enemiesSpawnGroupList == null) { return; }
        for (int i = 0; i < spawnPerFrame; i++)
        {

            if (enemiesSpawnGroupList.Count > 0)
            {
                if (enemiesSpawnGroupList[0].count <= 0) { return; }
                SpawnEnemy(enemiesSpawnGroupList[0].EnemyData, enemiesSpawnGroupList[0].isBoss);
                enemiesSpawnGroupList[0].count -= 1;
                if (enemiesSpawnGroupList[0].count <= 0)
                {
                    enemiesSpawnGroupList.RemoveAt(0);
                }
            }
        }
    }

    private void UpdateBossHealth()
    {
        if (bossEnemiesList == null) { return; }
        if (bossEnemiesList.Count == 0) { return; }
        currentBossHealth = 0;
        for (int i = 0; i < bossEnemiesList.Count; i++)
        {
            if (bossEnemiesList[i] == null)
            {
                continue;
            }
            currentBossHealth += bossEnemiesList[i].stats.hp;
        }

        bossHealthBar.value = currentBossHealth;

        if (currentBossHealth <= 0)
        {
            bossHealthBar.gameObject.SetActive(false);
            bossEnemiesList.Clear();
        }
    }
    public void AddGroupToSpawn(EnemyData enemyToSpawn, int count, bool isBoss)
    {
        EnemiesSpawnGroup newGroupToSpawn = new EnemiesSpawnGroup(enemyToSpawn, count, isBoss);
        if (enemiesSpawnGroupList == null)
        {
            enemiesSpawnGroupList = new List<EnemiesSpawnGroup>();
        }
        enemiesSpawnGroupList.Add(newGroupToSpawn);
    }

    public void SpawnEnemy(EnemyData enemyToSpawn, bool isBoss)
    {
        float yRandom = Random.Range(-spawnArea.y, spawnArea.y);
        float xRandom = Random.Range(-spawnArea.x, spawnArea.x);
        if (xRandom != spawnArea.x || xRandom != -spawnArea.x)
        {
            int rdNum = Random.Range(0, 100);
            yRandom = rdNum < 50 ? spawnArea.y : -spawnArea.y;
        }
        Vector3 spawnPosition = new Vector3(xRandom, yRandom, 0);

        spawnPosition += player.transform.position;

        //spawning main object
        GameObject enemySpawn = poolManager.GetObject(enemyToSpawn.poolObjectData);
        enemySpawn.transform.position = spawnPosition;

        enemySpawn.GetComponent<Enemy>().SetTarget(player);
        enemySpawn.GetComponent<Enemy>().SetStats(enemyToSpawn.stats);
        enemySpawn.GetComponent<Enemy>().UpdateStatsForProgress(stageProgress.Progress);

        if (isBoss == true)
        {
            SpawnBossEnemy(enemySpawn.GetComponent<Enemy>());
        }

        enemySpawn.transform.parent = transform;

        //spawning sprite
        //enemySpawn.GetComponent<Enemy>().InitSprite(enemyToSpawn.animatedPrefab);

    }

    private void SpawnBossEnemy(Enemy newBoss)
    {
        if (bossEnemiesList == null)
        {
            bossEnemiesList = new List<Enemy>();
        }
        bossEnemiesList.Add(newBoss);

        totalBossHealth += newBoss.stats.hp;

        bossHealthBar.gameObject.SetActive(true);
        bossHealthBar.maxValue = totalBossHealth;
    }

    public void AddRepeatedSpawn(StageEvent stageEvent, bool isBoss)
    {
        EnemiesSpawnGroup repeatSpawnGroup = new EnemiesSpawnGroup(stageEvent.enemyToSpawn, stageEvent.count, isBoss);
        repeatSpawnGroup.SetRepeatSpawn(stageEvent.repeatEverySecond, stageEvent.repeatCount);

        if (repeatedSpawnGroupList == null)
        {
            repeatedSpawnGroupList = new List<EnemiesSpawnGroup>();
        }
        repeatedSpawnGroupList.Add(repeatSpawnGroup);
    }
}

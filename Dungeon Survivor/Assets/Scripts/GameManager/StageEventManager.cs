using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEventManager : MonoBehaviour
{
    [SerializeField] StageData stageData;
    EnemyManager enemyManager;

    StageTime stageTime;
    int eventIndexer;
    PlayerWinManager playerWin;
    PlayerLoseManager playerLose;

    private void Awake()
    {
        stageTime = GetComponent<StageTime>();
    }

    private void Start()
    {
        playerWin = FindObjectOfType<PlayerWinManager>();
        playerLose = FindObjectOfType<PlayerLoseManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
    }
    private void Update()
    {
        if (eventIndexer >= stageData.stageEvents.Count)
        {
            return;
        }
        if (stageTime.time > stageData.stageEvents[eventIndexer].time)
        {
            switch (stageData.stageEvents[eventIndexer].eventType)
            {
                case StageEventType.SpawnEnemy:
                    SpawnEnemy(false);
                    break;
                case StageEventType.SpawnObject:
                    SpawnObject();
                    break;
                case StageEventType.WinStage:
                    WinStage();
                    break;
                case StageEventType.SpawnEnemyBoss:
                    SpawnEnemy(true);
                    break;
                case StageEventType.LoseStage:
                    LoseStage(stageData.stageEvents[eventIndexer].eventType);
                    break;
            }
            eventIndexer += 1;
        }
    }

    private void WinStage()
    {
        playerWin.Win(stageData.stageId);
    }
    private void LoseStage(StageEventType stageEventType)
    {
        if (enemyManager.poolManager.checkWinning(enemyManager.poolManager.poolList))
        {
            stageEventType = StageEventType.WinStage;
            WinStage();
        }
        else
        {
            playerLose.Lose();
        }
        //playerLose.Lose();
    }

    private void SpawnEnemy(bool bossEnemy)
    {
        StageEvent currentEvent = stageData.stageEvents[eventIndexer];
        enemyManager.AddGroupToSpawn(currentEvent.enemyToSpawn, currentEvent.count, bossEnemy);

        if (currentEvent.isRepeatedEvent)
        {
            enemyManager.AddRepeatedSpawn(currentEvent, bossEnemy);
        }
    }

    private void SpawnObject()
    {
        for (int i = 0; i < stageData.stageEvents[eventIndexer].count; i++)
        {
            Vector3 positionToSpawn = GameManager.instance.playerTranform.position;

            SpawnManager.instance.SpawnObject(stageData.stageEvents[eventIndexer].objectToSpawn);
        }

    }
}

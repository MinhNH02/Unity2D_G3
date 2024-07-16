using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public string enemyDataName;
    public PoolObjectData poolObjectData;
    public EnemyStats stats;
}

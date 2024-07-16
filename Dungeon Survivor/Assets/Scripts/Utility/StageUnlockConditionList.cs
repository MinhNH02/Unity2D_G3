using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageUnlockCondition
{
    public string condition;
    public bool state;
}

[CreateAssetMenu]
public class StageUnlockConditionList : ScriptableObject
{
    public List<StageUnlockCondition> conditionList;
    public StageUnlockCondition GetCondition(string condition)
    {
        return conditionList.Find(x => x.condition == condition);
    }
}

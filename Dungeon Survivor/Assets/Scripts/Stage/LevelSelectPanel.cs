using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectPanel : MonoBehaviour
{
    [SerializeField] List<SelectStageButton> stageSelectButton;
    [SerializeField] StageUnlockConditionList conditionList;
    [SerializeField] DataContainer dataContainer;
    void UpdateButtons()
    {
        for (int i = 0; i < stageSelectButton.Count; i++)
        {
            bool unlocked = UpdateButton(stageSelectButton[i]);
            stageSelectButton[i].gameObject.SetActive(unlocked);
        }
    }

    private bool UpdateButton(SelectStageButton stage)
    {
        bool unlocked = true;
        if (stage.stageData.stageCompletionToUnlock == null)
        {
            return unlocked;
        }
        for (int i = 0; i < stage.stageData.stageCompletionToUnlock.Count; i++)
        {
            string id = stage.stageData.stageCompletionToUnlock[i];
            if (conditionList.GetCondition(id).state == false)
            {
                unlocked = false;
            }
        }
        return unlocked;
    }

    private void OnEnable()
    {
        UpdateButtons();
    }
}

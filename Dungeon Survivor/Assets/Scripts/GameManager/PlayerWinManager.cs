using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWinManager : MonoBehaviour
{
    [SerializeField] GameObject winMessagePanel;
    PauseManager pauseManager;
    [SerializeField] DataContainer dataContainer;
    [SerializeField] StageUnlockConditionList conditionList;
    private void Start()
    {
        {
            pauseManager = GetComponent<PauseManager>();
        }
    }
    public void Win(string stageId)
    {
        winMessagePanel.SetActive(true);
        pauseManager.PauseGame();
        StageUnlockCondition f = conditionList.GetCondition(stageId);
        f.state = (f != null);
    }
}

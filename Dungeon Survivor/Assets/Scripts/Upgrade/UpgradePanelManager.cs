using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanelManager : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] UpgradeDescriptionPanel updgradeDescriptionPanel;
    PauseManager pauseManager;

    [SerializeField] List<UpgradeButton> upgradeButtons;

    Level characterLevel;
    int selectedUpgradeId;
    List<UpgradeData> upgradeData;
    private void Awake()
    {
        pauseManager = GetComponent<PauseManager>();
        characterLevel = GameManager.instance.playerTranform.GetComponent<Level>();
    }

    private void Start()
    {
        HideButton();
        selectedUpgradeId = -1;
    }

    public void OpenPanel(List<UpgradeData> upgradeDatas)
    {
        Clean();
        pauseManager.PauseGame();
        panel.SetActive(true);

        this.upgradeData= upgradeDatas;

        for (int i = 0; i < upgradeDatas.Count; i++)
        {
            upgradeButtons[i].gameObject.SetActive(true);
            upgradeButtons[i].Set(upgradeDatas[i]);
        }
    }

    public void Upgrade(int pressButtonId)
    {
        if (selectedUpgradeId != pressButtonId)
        {
            selectedUpgradeId = pressButtonId;
            ShowDescription();
        }
        else
        {
           characterLevel.Upgrade(pressButtonId);
            ClosePanel();
            HideDescription();
        }
    }

    private void HideDescription()
    {
        updgradeDescriptionPanel.gameObject.SetActive(false);

    }

    private void ShowDescription()
    {
        updgradeDescriptionPanel.gameObject.SetActive(true);
        updgradeDescriptionPanel.Set(upgradeData[selectedUpgradeId]);
    }

    public void Clean()
    {
        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            upgradeButtons[i].Clean();
        }
    }

    public void ClosePanel()
    {
        selectedUpgradeId = -1;
        HideButton();
        pauseManager.UnPauseGame();
        panel.SetActive(false);
    }

    private void HideButton()
    {
        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            upgradeButtons[i].gameObject.SetActive(false);
        }
    }
}

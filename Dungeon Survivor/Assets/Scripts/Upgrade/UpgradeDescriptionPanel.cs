using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeDescriptionPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI upgradeNameText;
    [SerializeField] TextMeshProUGUI updgradeDescription;

    public void Set(UpgradeData upgradeData)
    {
        upgradeNameText.text = upgradeData.upgradeName;
        updgradeDescription.text = upgradeData.upgradeDecription;
    }
}


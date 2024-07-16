using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItems : MonoBehaviour
{
    [SerializeField] List<Item> items;
    PlayerManager playerManager;
    public List<UpgradeData> upgradeData;
    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private void Start()
    {
    }

    public void Equip(Item itemToEquip)
    {
        if(items == null)
        {
            items = new List<Item>();
        }
        Item newItemInstance = new Item();
        newItemInstance.Init(itemToEquip.name);
        newItemInstance.stats.Sum(itemToEquip.stats);

        items.Add(newItemInstance);
        newItemInstance.Equip(playerManager);
    }


    internal void UpgradeItem(UpgradeData upgradeData)
    {
        Item itemToUpgrade = items.Find(id => id.name == upgradeData.item.name);
        itemToUpgrade.UnEquip(playerManager);
        itemToUpgrade.stats.Sum(upgradeData.itemStats);
        itemToUpgrade.Equip(playerManager);
    }
}

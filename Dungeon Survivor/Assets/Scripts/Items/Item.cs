using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemStats
{
    public int armor;

    internal void Sum(ItemStats stats)
    {
        armor += stats.armor;
    }
}

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string itemName;
    public ItemStats stats;
    public List<UpgradeData> upgrades;

    public void Init(string name)
    {
        this.name = name;
        stats = new ItemStats();
        upgrades = new List<UpgradeData>();
    }

    public void Equip(PlayerManager player)
    {
        player.armor += stats.armor;
    }

    public void UnEquip(PlayerManager player)
    {
        player.armor -= stats.armor;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour, IPickUpObject
{
    [SerializeField] int count;
    public void OnPickUp(PlayerManager player)
    {
        player.coins.Add(count);
    }
}

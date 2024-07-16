using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickupObject : MonoBehaviour, IPickUpObject
{
    [SerializeField] private int healAmount;
    public void OnPickUp(PlayerManager player)
    {
        player.Health(healAmount);
    }

}

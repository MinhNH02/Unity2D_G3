using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpPickupObject : MonoBehaviour, IPickUpObject
{
    [SerializeField] int amount;
    public void OnPickUp(PlayerManager player)
    {
        player.level.AddExperience(amount);
    }
}

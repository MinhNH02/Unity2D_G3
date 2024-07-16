using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerManager player = collision.GetComponent<PlayerManager>();
        if(player != null)
        {
            GetComponent<IPickUpObject>().OnPickUp(player);
            Destroy(gameObject);
        }
    }
}

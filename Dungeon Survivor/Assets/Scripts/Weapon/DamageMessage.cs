using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMessage : MonoBehaviour
{
    [SerializeField] float timeToLive = 1f;
    float tt1 = 1f;

    private void OnEnable()
    {
        tt1 = timeToLive;
    }

    private void Update()
    {
        tt1 -= Time.deltaTime;
        if(tt1 < 0f)
        {
            gameObject.SetActive(false);
        }
    }
}

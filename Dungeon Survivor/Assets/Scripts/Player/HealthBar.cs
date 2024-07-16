using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform healthBar;

    public void SetStatus(int currentHealth, int maxHealth)
    {
        float current = (float) currentHealth;
        current /= maxHealth;
        if(current <= 0) current = 0;

        healthBar.localScale = new Vector3(current, 5.5f, 1f);
    }
}

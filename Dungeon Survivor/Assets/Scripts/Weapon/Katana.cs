using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : WeaponBase
{
    [SerializeField] private GameObject katanaAttackLeft;
    [SerializeField] private GameObject katanaAttackRight;
    [SerializeField] float attackAreaSize = 3f;

    public override void Attack()
    {
        if (playerMovement.lastHorizontalDeCoupledVector < 0)
        {
            katanaAttackLeft.SetActive(true);
            Collider2D[] hit = Physics2D.OverlapCircleAll(katanaAttackLeft.transform.position, attackAreaSize);
            ApplyDamage(hit);
        }
        else
        {
            katanaAttackRight.SetActive(true);
            Collider2D[] hit = Physics2D.OverlapCircleAll(katanaAttackRight.transform.position, attackAreaSize);
            ApplyDamage(hit);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaver : WeaponBase
{
    [SerializeField] private GameObject cleaverAttackLeft;
    [SerializeField] private GameObject cleaverAttackRight;
    [SerializeField] float attackAreaSize = 3f;

    public override void Attack()
    {
        if (playerMovement.lastHorizontalDeCoupledVector < 0)
        {
            cleaverAttackLeft.SetActive(true);
            Collider2D[] hit = Physics2D.OverlapCircleAll(cleaverAttackLeft.transform.position, attackAreaSize);
            ApplyDamage(hit);
        }
        else
        {
            cleaverAttackRight.SetActive(true);
            Collider2D[] hit = Physics2D.OverlapCircleAll(cleaverAttackRight.transform.position, attackAreaSize);
            ApplyDamage(hit);
        }
    }
}

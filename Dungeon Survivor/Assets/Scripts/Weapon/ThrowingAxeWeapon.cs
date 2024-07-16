using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingAxeWeapon : WeaponBase
{
    [SerializeField] PoolObjectData axePrefab;

    public override void Attack()
    {
        UpdateVectorOfAttack();
        for (int i = 0; i < weaponStats.numberOfAttacks; i++)
        {
            Vector3 axePos = transform.position;

            SpawnProjectile(axePrefab, axePos);
        }

    }
}

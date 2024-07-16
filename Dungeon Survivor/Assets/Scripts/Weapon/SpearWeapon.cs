using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearWeapon : WeaponBase
{
    [SerializeField] PoolObjectData spearPrefab;

    public override void Attack()
    {
        UpdateVectorOfAttack();
        for (int i = 0; i < weaponStats.numberOfAttacks; i++)
        {
            Vector3 spearPos = transform.position;

            SpawnProjectile(spearPrefab, spearPos);
        }

    }
}

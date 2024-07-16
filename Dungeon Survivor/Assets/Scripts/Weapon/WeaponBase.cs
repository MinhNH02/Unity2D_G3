using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackDirection
{
    None,
    Forward,
    LeftRight,
    UpDown
}
public abstract class WeaponBase : MonoBehaviour
{
    public WeaponData weaponData;
    public WeaponStats weaponStats;
    float timer;

    public Player playerMovement;
    PlayerManager playerManager;
    public Vector2 vectorOfAttack;
    [SerializeField] AttackDirection attackDirection;
    PoolManager poolManager;

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            Attack();
            timer = weaponStats.timeToAttack;
        }
    }
    private void Awake()
    {
        playerMovement = GetComponentInParent<Player>();
    }
    public virtual void SetData(WeaponData wd)
    {
        weaponData = wd;

        weaponStats = new WeaponStats(wd.stats);
    }
    public void SetPoolManager(PoolManager poolManager)
    {
        this.poolManager = poolManager;
    }
    public abstract void Attack();
    public void ApplyDamage(Collider2D[] hit)
    {
        int damage = GetDamage();
        for (int i = 0; i < hit.Length; i++)
        {
            IDamageable enemy = hit[i].GetComponent<IDamageable>();
            if (enemy != null)
            {
                ApplyDamage(hit[i].transform.position, damage, enemy);
            }
        }
    }

    public void ApplyDamage(Vector3 position, int damage, IDamageable enemy)
    {
        PostDamage(damage, position);
        enemy.TakeDamage(damage);
        ApplyAdditionalEffects(enemy, position);
    }

    private void ApplyAdditionalEffects(IDamageable enemy, Vector3 enemyPosition)
    {
        enemy.Stun(weaponStats.stun);
        enemy.Knockback((enemyPosition - transform.position).normalized, weaponStats.knockback, weaponStats.knockbackTimeWeight);
    }

    public int GetDamage()
    {
        int damage = (int)(weaponData.stats.damage * playerManager.damageBonus);
        return damage;
    }

    public virtual void PostDamage(int damage, Vector3 targetPosition)
    {
        MessageSystem.instance.PostMessage(damage.ToString(), targetPosition);
    }

    public void Upgrade(UpgradeData upgradeData)
    {
        weaponStats.Sum(upgradeData.weaponUpgradeStats);
    }

    internal void AddOwnerCharacter(PlayerManager character)
    {
        playerManager = character;
    }
    public void UpdateVectorOfAttack()
    {
        if (attackDirection == AttackDirection.None)
        {
            vectorOfAttack = Vector2.zero;
            return;
        }
        switch (attackDirection)
        {
            case AttackDirection.Forward:
                vectorOfAttack.x = playerMovement.lastHorizontalCoupledVector;
                vectorOfAttack.y = playerMovement.lastVerticalCoupledVector;
                break;
            case AttackDirection.UpDown:
                vectorOfAttack.x = 0f;
                vectorOfAttack.y = playerMovement.lastVerticalDeCoupledVector;
                break;
            case AttackDirection.LeftRight:
                vectorOfAttack.x = playerMovement.lastHorizontalDeCoupledVector;
                vectorOfAttack.y = 0f;
                break;
        }
        vectorOfAttack = vectorOfAttack.normalized;
    }
    public GameObject SpawnProjectile(PoolObjectData poolObjectData, Vector3 position)
    {
        GameObject projectileGO = poolManager.GetObject(poolObjectData);
        projectileGO.transform.position = position;

        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.setDirection(vectorOfAttack.x, vectorOfAttack.y);
        projectile.SetStats(this);

        return projectileGO;
    }

}

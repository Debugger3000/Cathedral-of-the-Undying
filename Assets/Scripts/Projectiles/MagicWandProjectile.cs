using System;
using UnityEngine;

public class MagicWandProjectile : BaseProjectile
{

    public float spreadAngle = 15f;
    public int splitCount = 5;

    // NO EFFECT since our projectile doesnt
    public override Tuple<WeaponDebuffData, StatsCopy> SpecialEffect(GameObject enemy, StatsCopy enemyStats, BaseProjectile projectile)
    {

        float debuffedMoveSpeed = enemyStats.moveSpeed - (enemyStats.moveSpeed * debuffData.effectIntensity); 
        StatsCopy debuffedStats = new StatsCopy(enemyStats.currentHealth, debuffedMoveSpeed, enemyStats.armour);

        return new Tuple<WeaponDebuffData, StatsCopy>(debuffData, debuffedStats);
    }

    protected override void DistanceEffect()
    {
        Debug.Log($"Distance effect has triggered........");
        // do something
        for (int i = 0; i < 2; i++)
        {
            float angleOffset = (i == 0) ? -spreadAngle : spreadAngle;
            Quaternion spread = Quaternion.Euler(0, 0, angleOffset);
            Quaternion finalRotation = transform.rotation * spread;

            GameObject splitBullet = Instantiate(childrenProjectiles[0], transform.position, finalRotation);
            BaseProjectile bullet = splitBullet.GetComponent<BaseProjectile>();
            bullet.SetAttributes(speed, damage, armourPenetration, weaponPoints, isSpecialEffect, debuffData, false, distanceForDistanceEffect,childrenProjectiles);
        }
    }
}

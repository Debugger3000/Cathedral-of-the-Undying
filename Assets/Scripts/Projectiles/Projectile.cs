using System;
using UnityEngine;

public class Projectile : BaseProjectile
{


    //
    public override Tuple<WeaponDebuffData, StatsCopy> SpecialEffect(GameObject enemy, StatsCopy enemyStats, BaseProjectile projectile)
    {

        float debuffedMoveSpeed = enemyStats.moveSpeed - (enemyStats.moveSpeed * debuffData.effectIntensity); 
        StatsCopy debuffedStats = new StatsCopy(enemyStats.currentHealth, debuffedMoveSpeed, enemyStats.armour);

        return new Tuple<WeaponDebuffData, StatsCopy>(debuffData, debuffedStats);
    }
}

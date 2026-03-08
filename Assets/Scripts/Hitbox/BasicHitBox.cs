using System;
using UnityEngine;

public class BasicHitBox : AttackHitboxController
{
    
    // implement its special effect here...
    public override Tuple<WeaponDebuffData, StatsCopy> HitBoxSpecialEffect(GameObject enemy, StatsCopy enemyStats)
    {

        float debuffedMoveSpeed = enemyStats.moveSpeed - (enemyStats.moveSpeed * debuffData.effectIntensity); 
        StatsCopy debuffedStats = new StatsCopy(enemyStats.currentHealth, debuffedMoveSpeed, enemyStats.armour);

        return new Tuple<WeaponDebuffData, StatsCopy>(debuffData, debuffedStats);
    }
}

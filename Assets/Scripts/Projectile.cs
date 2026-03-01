using UnityEngine;

public class Projectile : BaseProjectile
{


    //
    public override DebuffController SpecialEffect(GameObject enemy, StatsCopy enemyStats, BaseProjectile projectile)
    {
        Debug.Log("Basic projectile no effect...");
        // divide movespeed by 3.... lmao
        float debuffedMoveSpeed = enemyStats.moveSpeed / debuffData.effectIntensity; // half movement speed for person hit

        StatsCopy debuffedStats = new StatsCopy(enemyStats.currentHealth, debuffedMoveSpeed, enemyStats.armour);


        // have to provide a lifetime from this projectiles special effect
        
        return new DebuffController(debuffData, debuffedStats);
    }  
}

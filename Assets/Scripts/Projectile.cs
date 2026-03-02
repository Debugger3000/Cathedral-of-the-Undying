using UnityEngine;

public class Projectile : BaseProjectile
{


    //
    public override DebuffController SpecialEffect(GameObject enemy, StatsCopy enemyStats, BaseProjectile projectile)
    {
        Debug.Log("Basic projectile no effect...");
        
        
        // divide movespeed by 3.... lmao
        // this calculation should be extracted to a static functino somewhere
        // this should be 70% movespeed
        float debuffedMoveSpeed = enemyStats.moveSpeed - (enemyStats.moveSpeed * debuffData.effectIntensity); // half movement speed for person hit

        StatsCopy debuffedStats = new StatsCopy(enemyStats.currentHealth, debuffedMoveSpeed, enemyStats.armour);


        // have to provide a lifetime from this projectiles special effect
        
        return new DebuffController(debuffData, debuffedStats);
    }  
}

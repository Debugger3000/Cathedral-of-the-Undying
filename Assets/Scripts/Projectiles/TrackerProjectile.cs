using System;
using UnityEngine;

public class TrackerProjectile : BaseProjectile
{
    public float projectileTrackingRotation;

    // NO EFFECT since our projectile doesnt
    public override Tuple<WeaponDebuffData, StatsCopy> SpecialEffect(GameObject enemy, StatsCopy enemyStats, BaseProjectile projectile)
    {

        float debuffedMoveSpeed = enemyStats.moveSpeed - (enemyStats.moveSpeed * debuffData.effectIntensity); 
        StatsCopy debuffedStats = new StatsCopy(enemyStats.currentHealth, debuffedMoveSpeed, enemyStats.armour);

        return new Tuple<WeaponDebuffData, StatsCopy>(debuffData, debuffedStats);
    }

    // this is run within update...
    protected override void ProjectileMovement()
    {
        // if (playerTarget == null) 
        // {
        //     // If no target, just keep going straight
        //     rb.linearVelocity = transform.up * speed;
        //     return;
        // }

        // get the direction from the projectile to the player
        Vector2 direction = (Vector2)playerTarget.position - rb.position;
        direction.Normalize();
        // compare vectors from projectile to target
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        // aply rotation
        rb.angularVelocity = -rotateAmount * projectileTrackingRotation;
        // constant forward movement
        rb.linearVelocity = transform.up * speed;


    }
}

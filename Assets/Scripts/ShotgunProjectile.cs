using System;
using UnityEngine;

public class ShotgunProjectile : BaseProjectile
{

    public float knockbackForce; // knockback for shotgun pellets

    // half movement speed for person hit


    // this is shotgun projectile
    // shoot 3 bullets
    // knockback effect
    public override Tuple<WeaponDebuffData, StatsCopy> SpecialEffect(GameObject enemy, StatsCopy enemyStats, BaseProjectile projectile)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // push unit back opposite to projectile hit on unit collider
            Vector2 knockbackDirection = (enemy.transform.position - projectile.transform.position).normalized;
            // Apply impulse push back on unit
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            Debug.Log($"knockback force is: {knockbackForce}");
            
        }
        float debuffedMoveSpeed = enemyStats.moveSpeed - (enemyStats.moveSpeed * debuffData.effectIntensity);
        StatsCopy debuffedStats = new StatsCopy(enemyStats.currentHealth, debuffedMoveSpeed, enemyStats.armour);

        return new Tuple<WeaponDebuffData, StatsCopy>(debuffData, debuffedStats);
    }
}

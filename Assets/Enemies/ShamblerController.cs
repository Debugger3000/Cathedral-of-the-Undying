using UnityEngine;
public class ShamblerController : EnemyController
{
    // enemy units implement own death
    protected override void Die()
    {
        // Shamblers might explode or do something on death...
        Destroy(gameObject);
    }

    protected override void ExecuteAttackLogic()
    {
        // Debug.Log("Shambler performs a Cone Slam!");
        Debug.Log($"Enemy is using its basic attack.... stage 2 hitbox instantiate");

        // 1. Get the rotation
        Quaternion spawnRotation = transform.rotation;

        // 2. Calculate a position slightly in front of the enemy face
        // 'transform.up' is the direction the enemy is facing. 
        // Multiply by 0.5f or 1.0f to push it out.
        Vector3 spawnPosition = transform.position + (transform.up * 1.5f);

        GameObject hitbox = Instantiate(enemyData.attackHitbox, spawnPosition, spawnRotation); // generate hitbox

        hitbox.GetComponentInChildren<AttackHitboxController>().Setup(enemyData.damage, enemyData.hitboxLifetime, enemyData.isHitBoxSpecialEffect, enemyData.debuffDataHitBox);
        
        Destroy(hitbox,1f); // destroy hitbox after attack...
    }

    // on hitbox hit, we apply special effect..
    protected override void SpecialHitBoxAttackEffect(GameObject player, StatsCopy playerStats)
    {
        
    }

    // on projectile hit, apply enemies special effects...
    // protected override void SpecialProjectileEffect(GameObject player, StatsCopy playerStats, BaseProjectile projectile)
    // {
        
    // }

}

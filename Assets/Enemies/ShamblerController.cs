using UnityEngine;
public class ShamblerController : EnemyController
{
    // enemy units implement own death
    protected override void Die()
    {
        // Shamblers might explode or leave a puddle
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

        hitbox.GetComponentInChildren<AttackHitboxController>().Setup(enemyData.damage, enemyData.hitboxLifetime);
        // // The logic you wanted: Check circle then filter by angle
        // Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, enemyData.attackRange, LayerMask.GetMask("Player"));

        // foreach (var player in hitPlayers)
        // {
        //     Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
            
        //     // Using the forward vector (transform.up) to see if player is in the "slice"
        //     if (Vector3.Angle(transform.up, dirToPlayer) < enemyData.attackAngle / 2f)
        //     {
        //         // player.GetComponent<PlayerHealth>().TakeDamage(enemyData.damage);
        //         Debug.Log("Shambler hit the player with a cone attack!");
        //     }
        // }
        //Destroy(hitbox,2f); // destroy hitbox after attack...
    }
}

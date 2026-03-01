using UnityEngine;

public class ShotgunProjectile : BaseProjectile
{

    public float knockbackForce;
    // this is shotgun projectile
    // shoot 3 bullets
    // knockback effect
    public override EnemyStatsCopy SpecialEffect(GameObject enemy, EnemyStatsCopy enemyStats, BaseProjectile projectile)
    {

        // Debug.Log("The fireball exploded in a radius!");
        // Add explosion particles or area-of-effect damage here

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // 2. Calculate direction (Away from the fireball/impact)
            // Note: You might need to pass the explosion source position into this method
            // For now, we'll assume a standard backward push relative to the enemy's current face
            // Vector2 knockbackDirection = -enemy.transform.right; // Or (enemy.transform.position - explosionSource).normalized;
            Vector2 knockbackDirection = (enemy.transform.position - projectile.transform.position).normalized;
            //float knockbackForce = 25f; // This could also be a stat in your EnemyData

            // 3. Apply the force
            // ForceMode2D.Impulse is best for instant "hits" like explosions
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            Debug.Log($"knockback force is: {knockbackForce}");
        }


        return enemyStats;
    }
}

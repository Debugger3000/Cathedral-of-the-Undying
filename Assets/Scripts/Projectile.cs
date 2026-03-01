using UnityEngine;

public class Projectile : BaseProjectile
{
    
    

    // void Awake()
    // {
    //     // rb = GetComponent<Rigidbody2D>();
    //     // SetAttributes();
    // }

    //
    public override EnemyStatsCopy SpecialEffect(GameObject enemy, EnemyStatsCopy enemyStats, BaseProjectile projectile)
    {
        Debug.Log("Basic projectile no effect...");
        
        return enemyStats;
    }

    // Use FixedUpdate for physics-based velocity
    // void FixedUpdate()
    // {
    //     // We set velocity directly to ensure it moves at a constant speed
    //     // rb.linearVelocity = transform.up * speed; 
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    
}

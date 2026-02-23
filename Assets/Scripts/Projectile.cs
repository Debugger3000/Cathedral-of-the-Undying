using UnityEngine;

public class Projectile : BaseProjectile
{
    
    

    // void Awake()
    // {
    //     // rb = GetComponent<Rigidbody2D>();
    //     // SetAttributes();
    // }

    //
    public override void SpecialEffect()
    {
        Debug.Log("The fireball exploded in a radius!");
        // Add explosion particles or area-of-effect damage here
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

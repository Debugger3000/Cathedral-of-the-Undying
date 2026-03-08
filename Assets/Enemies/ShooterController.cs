using UnityEngine;

public class ShooterController : EnemyController
{
    
    protected override void Die()
    {
        // Shamblers might explode or do something on death...
        Destroy(gameObject);
    }
}

using UnityEngine;
public class ShamblerController : EnemyController
{
    // enemy units implement own death
    protected override void Die()
    {
        // Shamblers might explode or leave a puddle
        Destroy(gameObject);
    }
}

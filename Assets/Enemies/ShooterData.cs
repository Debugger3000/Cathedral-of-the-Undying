using UnityEngine;

[CreateAssetMenu(fileName = "Enemies", menuName = "Enemies/Shooter")]
public class ShooterData : EnemyData 
{
    // Shambler most basic unit...
    // Only needs default stats with no special overrides

    // OVERRIDE DEFAULT ENEMY BEHAVIOUR WITHIN HERE

    public override void AttackController(Transform transform, Transform playerTarget)
    {
        BasicProjectileFire(transform,playerTarget); // just perform a basic projectile attack
    }
}

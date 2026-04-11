using UnityEngine;

[CreateAssetMenu(fileName = "Enemies", menuName = "Enemies/Shooter")]
public class ShooterData : EnemyData 
{
    // Shambler most basic unit...
    // Only needs default stats with no special overrides

    // OVERRIDE DEFAULT ENEMY BEHAVIOUR WITHIN HERE

    public override string AttackController(Transform transform, Transform playerTarget, MonoBehaviour owner)
    {
        BasicProjectileFire(transform,playerTarget); // just perform a basic projectile attack
        return "attack";
    }
}

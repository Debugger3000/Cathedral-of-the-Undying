using UnityEngine;

[CreateAssetMenu(fileName = "Enemies", menuName = "Enemies/Tracker")]
public class TrackerData : EnemyData 
{
    // Shambler most basic unit...
    // Only needs default stats with no special overrides
    

    // OVERRIDE DEFAULT ENEMY BEHAVIOUR WITHIN HERE

    public override void AttackController(Transform transform, Transform target)
    {
        BasicProjectileFire(transform, target); // leave this the same since we just want to shoot one projectile
    }
}

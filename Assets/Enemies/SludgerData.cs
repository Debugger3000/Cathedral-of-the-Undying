
using UnityEngine;

[CreateAssetMenu(fileName = "Enemies", menuName = "Enemies/Sludger")]
public class SludgerData : EnemyData
{
    // Vars
    private int numberOfHitBoxes = 3;
    //private int delayBetweenHitBoxes = 3;
    private float distanceScale = 3f;

    // OVERRIDE
    // Implement sludgers unique hitbox attack
    public override void BasicHitBoxAttack(Transform transform, Transform target)
    {

        // Get the rotation
        Quaternion spawnRotation = transform.rotation;

        for(int i = 0; i < numberOfHitBoxes; i++)
        {
            // 2. Calculate a position slightly in front of the enemy face
            // 'transform.up' is the direction the enemy is facing. 
            // Multiply by 0.5f or 1.0f to push it out.
            Vector3 spawnPosition = target.position + (transform.up * ((i + 1)*distanceScale));

            GameObject hitbox = Instantiate(attackHitbox, spawnPosition, spawnRotation); // generate hitbox

            hitbox.GetComponentInChildren<AttackHitboxController>().Setup(damage, armourPenetration, hitboxLifetime, isHitBoxSpecialEffect, debuffDataHitBox);
            
            Destroy(hitbox,hitboxLifetime); // destroy hitbox after attack...
        }

        
    }
}

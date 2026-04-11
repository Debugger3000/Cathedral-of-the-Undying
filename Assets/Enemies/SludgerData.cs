
using UnityEngine;

[CreateAssetMenu(fileName = "Enemies", menuName = "Enemies/Sludger")]
public class SludgerData : EnemyData
{
    // Vars
    private int numberOfHitBoxes = 3;
    //private int delayBetweenHitBoxes = 3;
    private float distanceScale = 2f;

    // OVERRIDE
    // Implement sludgers unique hitbox attack
    public override void BasicHitBoxAttack(Transform transform, Transform target, MonoBehaviour owner)
    {

        // Get the rotation
        Quaternion spawnRotation = transform.rotation;
        Vector3 spawnPosition;

        for(int i = 0; i < numberOfHitBoxes; i++)
        {
            // 2. Calculate a position slightly in front of the enemy face
            // 'transform.up' is the direction the enemy is facing. 
            // Multiply by 0.5f or 1.0f to push it out.
            if (i == 0)
            {
                spawnPosition = target.position;
            }
            else
            {
                spawnPosition = target.position + (transform.up * (distanceScale * i));
            }

            GameObject hitbox = Instantiate(attackHitbox, spawnPosition, spawnRotation); // generate hitbox

            hitbox.GetComponentInChildren<AttackHitboxController>().Setup(damage, armourPenetration, 0, hitboxLifetime, isHitBoxSpecialEffect, debuffDataHitBox);
            
            Destroy(hitbox,hitboxLifetime); // destroy hitbox after attack...
        }

        
    }
}

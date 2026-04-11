using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Enemies", menuName = "Enemies/Demon")]
public class DemonData : EnemyData
{

    [Header("Demon Variables")]
    public float warningExplosionTime;
    [SerializeField]
    public float beamAttackDuration = 5f; // 5 seconds
    public float beamRotationSpeed = 0.25f;
    public float beamAttackCoolDown = 5.1f;

    // Vars
    

    // Attacks Order
    // attackHitboxList[0] = circle attack warning hitbox
    // attackHitboxList[1] = beam attack hitbox



    // Attacks
        // AOE poison around Demon if you get too close

        // Attack 0
        // Spawns hit box on target location, where player has time to escape, if movment is good...

        // Attack 1
        // Kamhuhuhuhuhuha beam
            // Start at player position and slowly tracks player position
            // Player with max movement speed should easily be able to run outside of it
    



    public override void BasicHitBoxAttack(Transform transform, Transform target,MonoBehaviour owner)
    {

        // if ()
        // {
            
        // }
        // Run attack Queue
        // int currentAttack = attackQueue.Dequeue();

        // which attacks to use based on queue value
        if (attackIndex == 0)
        {
            // use circle target attack
            TargetCircleAttack(transform,target,owner);
        }
        else if(attackIndex == 1)
        {
            // use beam attack
            BeamAttack(transform,target);
        }

        // // dequeue after attack if count is above 0
        // if(attackQueue.Count > 0)
        // {
        //     attackQueue.Dequeue(); // remove last index
        // }
        // if we got empty Queue, we add new randomized attacks...
        if(attackQueue.Count == 0)
        {
            RandomizeAttack(); // add attacks into queue
        }
    }

    private void TargetCircleAttack(Transform transform, Transform target,MonoBehaviour owner)
    {
        owner.StartCoroutine(TargetCircleAttackRoutine(transform, target));
        
    }

    private IEnumerator TargetCircleAttackRoutine(Transform transform, Transform target)
    {
        // Spawn warning indicator (harmless)
        Vector3 attackPosition = target.position;
        GameObject warning = Instantiate(attackHitboxList[0], attackPosition, transform.rotation);
        Destroy(warning,warningExplosionTime); // destroy always
        // Wait for player to react
        yield return new WaitForSeconds(warningExplosionTime);
        
        

        // Spawn actual damaging hitbox at same position
        GameObject hitbox = Instantiate(attackHitbox, attackPosition, transform.rotation);
        hitbox.GetComponentInChildren<AttackHitboxController>().Setup(damage, armourPenetration, 0, hitboxLifetime, isHitBoxSpecialEffect, debuffDataHitBox);
        
        Destroy(hitbox, hitboxLifetime);
    }

    private void BeamAttack(Transform transform, Transform target)
    {

        // Spawn warning indicator (harmless)
        Vector3 transformPosition = transform.position + (transform.up * 5f);
        activeHitbox = Instantiate(attackHitboxList[1], transformPosition, transform.rotation, transform);
        Debug.Log($"Beam spawned SPAWNED: {activeHitbox != null}");
        // Spawn actual damaging hitbox at same position
        activeHitbox.GetComponentInChildren<AttackHitboxController>().Setup(damage, armourPenetration, 0, beamAttackDuration, isHitBoxSpecialEffect, debuffDataHitBox);
        
    }

    public override void DefaultRotation(Transform target, Transform bodyTransform, float rotationSpeed = 3f)
    {   

        float speed = rotationSpeed > 0 ? rotationSpeed : 0.25f;
    
        Vector2 direction = (target.position - bodyTransform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        float angle = Mathf.LerpAngle(bodyTransform.eulerAngles.z, targetAngle, speed * Time.deltaTime);
        bodyTransform.rotation = Quaternion.Euler(0, 0, angle);
    }


    public override void EnemyStopsAttack(Transform transform, Transform target, MonoBehaviour owner)
    {
        if (activeHitbox != null)
        {
            Destroy(activeHitbox);
            activeHitbox = null;
        }
    }


}

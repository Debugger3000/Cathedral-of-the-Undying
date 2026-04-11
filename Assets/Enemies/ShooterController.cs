using System.Collections;
using UnityEngine;

public class ShooterController : EnemyController
{
    
    protected override void Die()
    {
        // Shamblers might explode or do something on death...
        Destroy(gameObject);
    }

    // if child enemy class wants to change their attack Sequence they can...
    protected override IEnumerator AttackSequence(Transform bodyTransform)
    {
        isAttacking = true;
        
        // 1. Wind up - Every enemy pauses briefly
        yield return new WaitForSeconds(enemyData.windUpTime);

        // 
        //ExecuteAttackLogic();
        enemyData.AttackController(bodyTransform,target,this); // call unit attack controller..

        // 3. Recovery / Cooldown
        yield return new WaitForSeconds(enemyData.attackCooldown);
        
        nextAttackTime = Time.time + enemyData.attackCooldown;
        isAttacking = false;
    }
}

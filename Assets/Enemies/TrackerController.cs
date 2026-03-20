using System.Collections;
using UnityEngine;

public class TrackerController : EnemyController
{
    protected override void Die()
    {
        // Shamblers might explode or do something on death...
        Destroy(gameObject);
    }

    // if child enemy class wants to change their attack Sequence they can...
    protected override IEnumerator AttackSequence()
    {
        isAttacking = true;
        
        // 1. Wind up - Every enemy pauses briefly
        yield return new WaitForSeconds(enemyData.windUpTime);

        // 
        //ExecuteAttackLogic();
        enemyData.AttackController(transform, target); // call unit attack controller.. from enemyData / unitData

        // 3. Recovery / Cooldown
        yield return new WaitForSeconds(enemyData.attackCooldown);
        
        nextAttackTime = Time.time + enemyData.attackCooldown;
        isAttacking = false;
    }
}

using System.Collections;
using UnityEngine;

public class DemonController : EnemyController
{
    protected override void Die()
    {
        AudioManager.Instance.PlayAudioClip(AudioKey.DemonDied);
        // Shamblers might explode or do something on death...
        Destroy(gameObject);
    }

    // if child enemy class wants to change their attack Sequence they can...
    protected override IEnumerator AttackSequence(Transform bodyTransform)
    {
        // if(enemyData.attackQueue.Count == 0)
        // {
        //     enemyData.RandomizeAttack(); // randomize 3 attacks in queue for demon...
        // }
        //Debug.Log("DEMON IS ATTACKING SEQUENCE RUNNSSSSSSSSSSSSSSSSSSSSSSSS");
        int attackIndex = enemyData.attackQueue.Dequeue();
        enemyData.attackIndex = attackIndex;
        // Do AOE Splash attack
        // Attack Sequence
        if (attackIndex == 0)
        {
            isAttacking = true;
        
            // 1. Wind up - Every enemy pauses briefly
            yield return new WaitForSeconds(enemyData.windUpTime);

            // 
            //ExecuteAttackLogic();
            enemyData.AttackController(bodyTransform,target, this); // call unit attack controller..

            // 3. Recovery / Cooldown
            yield return new WaitForSeconds(enemyData.attackCooldown);
            
            nextAttackTime = Time.time + enemyData.attackCooldown;
            isAttacking = false;
            
        }
        // Do Beam Attack
        // Attack Sequence
        else if (attackIndex == 1)
        {
            isAttacking = true;
            yield return new WaitForSeconds(enemyData.windUpTime);

            // Start beam
            enemyData.AttackController(bodyTransform, target, this);

            // beam lasts 5 seconds
            yield return new WaitForSeconds(5f);

            // Stop beam
            enemyData.EnemyStopsAttack(bodyTransform, target, this);

            yield return new WaitForSeconds(enemyData.attackCooldown); // cooldown wait till next attack

            nextAttackTime = Time.time + enemyData.attackCooldown;
            isAttacking = false;
        }

        
        
    }



    protected override IEnumerator StopAttack(Transform bodyTransform)
    {
        isAttacking = false; // attack should stop...

        enemyData.EnemyStopsAttack(bodyTransform,target,this); // stop attack
        // go on cooldown when we stop attack
        yield return new WaitForSeconds(enemyData.attackCooldown);
    }
}

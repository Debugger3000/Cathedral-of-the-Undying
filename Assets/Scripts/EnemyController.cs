// using System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

// Base enemy implementation
public abstract class EnemyController : MonoBehaviour
{
    // Base Settings
    protected Transform target; // player transform
    public Transform bodyTransform; // for body sprite renderer of enemy
    private Rigidbody2D rb;
    public EnemyData enemyData; // Drag your ShamblerData or BossData here

    private StatsCopy statsCopy; // copy of data to apply effects too...
    // public List<ActiveDebuff> activeDebuffs = new List<ActiveDebuff>();

    private DebuffController enemyDebuffController;

    // apply debuff to data, set a timer for that effect, in update check array for timers hitting zero, normalize 

    protected float nextAttackTime;
    protected bool isAttacking = false;

    private bool isAffected = false; // for special effects 

    // Unit Stats
    protected float currentHealth;

    [Header("UI")]
    public GameObject healthBarPrefab; // healthbar prefab
    public Image healthBarFill; // healthbar fill
    [SerializeField] private Transform effectTray; // 
    private Dictionary<string, GameObject> activeIcons = new(); // hold queue of icons...
    // private bool isFiring = false;

    
    
    // On enemy unit awake...
    void Awake()
    {
        //myTransform = transform; // assign own transform
        rb = GetComponent<Rigidbody2D>(); // get rigidbody
        target = GameObject.FindGameObjectWithTag("Player").transform; // get player transform to follow...
        currentHealth = enemyData.maxHealth; // set health...
        statsCopy = new StatsCopy(enemyData.maxHealth, enemyData.moveSpeed, enemyData.armour); // set enemy stats copy
        enemyDebuffController = new DebuffController(statsCopy); // debuff controller for enemy unit
    }

    // Child class Implemented Methods
    protected abstract void Die(); // unit death
    protected abstract IEnumerator AttackSequence(Transform bodyTransform); // children define their attack sequence
    protected virtual IEnumerator StopAttack(Transform bodyTransform)
    {
    
        isAttacking = false; // attack should stop...
        // go on cooldown when we stop attack
        yield return new WaitForSeconds(enemyData.attackCooldown);
    }

    

    // protected virtual IEnumerator SecondaryAttackSequence(Transform bodyTransform)
    // {
    //     // do secondary attack...
    //     yield return new WaitForSeconds(enemyData.attackCooldown);
    // }

    //------
    // OnTrigger 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Enemy unit hit by something");
        // Player projectile hit enemy 
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Projectile")
        {
            // Debug.Log("Hit an environment Layer!");

            BaseProjectile projectile = collision.GetComponent<BaseProjectile>(); // grab projectiles damage

            TakeDamage(projectile.damage, projectile.armourPenetration); // unit should lose health...

            // check if projetile / weapon has a special effect to apply...
            if(projectile.isSpecialEffect)
            {
                StartCoroutine(ApplySpecialEffectProjectile(projectile));                
            }
        }
        else if(LayerMask.LayerToName(collision.gameObject.layer) == "PlayerAttack")
        {
            if (collision.TryGetComponent<AttackHitboxController>(out var attackHitboxScript))
            {
                //Debug.Log($"Hit by {collision.gameObject.name} for {attackHitboxScript.damage} damage!");

                TakeDamage(attackHitboxScript.damage, attackHitboxScript.armourPenetration); // player takes damage

                // apply whatever special effects of hitbox attack to player
                // check if enemy has hitbox Special effects
                if (attackHitboxScript.isSpecialEffect)
                {
                    StartCoroutine(HitBoxSpecialEffect(attackHitboxScript)); // apply special effects
                }
            }
        }
        
    }

    //------
    // Projectile special effect ONTO enemy unit
    private IEnumerator ApplySpecialEffectProjectile(BaseProjectile projectile)
    {
        isAffected = true; 
        
        // Apply the knockback/effect and set statsCopy
        // projectile special effects can either debuff a unit or apply whatever logic at end of projectile
        Tuple<WeaponDebuffData, StatsCopy> debuffTuple = projectile.SpecialEffect(gameObject, statsCopy, projectile);

        WeaponDebuffData data = debuffTuple.Item1;
        StatsCopy stats = debuffTuple.Item2;
        enemyDebuffController.SetDebuff(data,stats);

        // Affect enemy UI with debuff
        AddEffect(data);

        // Wait for a short duration so the physics force actually moves the object
        yield return new WaitForSeconds(0.2f); 
        
        isAffected = false;
    }

    // Debuff / Special effects on attacks
    private IEnumerator HitBoxSpecialEffect(AttackHitboxController attackHitboxScript)
    {
        isAffected = true; 
        
        // Apply the knockback/effect and set statsCopy
        // projectile special effects can either debuff a unit or apply whatever logic at end of projectile
        Tuple<WeaponDebuffData, StatsCopy> debuffTuple = attackHitboxScript.HitBoxSpecialEffect(gameObject, statsCopy);

        WeaponDebuffData data = debuffTuple.Item1;
        StatsCopy stats = debuffTuple.Item2;
        enemyDebuffController.SetDebuff(data,stats);

        // Affect enemy UI with debuff
        AddEffect(data);

        // Wait for a short duration so the physics force actually moves the object
        yield return new WaitForSeconds(0.2f); 
        
        isAffected = false;
    }

    //------
    // Update lifetime 
    void Update()
    {
        if (target == null) return;

        // Always rotate toward player (even while attacking)
        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero; // stop movement
            enemyData.DefaultRotation(target, bodyTransform, 0.5f);
            return;
        }
        else
        {
            enemyData.DefaultRotation(target, bodyTransform);
        }


        var (environmentDetected, openAngle) = enemyData.EnvironmentDetection(bodyTransform);

        if (environmentDetected)
        {
            enemyData.EnvironmentAvoidanceRotation(target, bodyTransform, openAngle);
            enemyData.AvoidanceMovement(bodyTransform, rb, statsCopy.moveSpeed);
        }
        else
        {
            // enemyData.DefaultRotation(target, bodyTransform);
            enemyData.DefaultMovement(target, transform, rb, statsCopy.moveSpeed);
        }

        // Attack check
        if (Time.time >= nextAttackTime)
        {
            Debug.Log($"ISaTTACKING: {isAttacking}");
            RaycastHit2D hit = enemyData.DefaultDetection(bodyTransform, enemyData);
            if (hit.collider != null && !isAttacking)
            {
                Debug.Log($"DEMON ATTACK............");
                StartCoroutine(AttackSequence(bodyTransform));
                return; // attacking, don't move
            }
            // else if(hit.collider != null && isAttacking)
            // {
            //     Debug.Log($"DEMONNN Stop attack....");
            //     // weaponControl.PlayerStopsAttackWithWeapon(transform);
            //     StartCoroutine(StopAttack(bodyTransform));
            // }
        }

        // check debuff status
        // HandleDebuffTimers(); 
        if(enemyDebuffController.activeDebuffs.Count > 0)
        {
            var (updatedStats, dotDmg) = enemyDebuffController.HandleDebuffTimers("enemy");
            statsCopy = updatedStats;
            if (statsCopy.enemyEffectsToRemove.Count > 0)
            {
                RemoveEffect(statsCopy.enemyEffectsToRemove);
            }

            if (dotDmg != 0){
                FlatTakeDamage(dotDmg);
            }
        }
    }

    // receive flat damage...
    public void TakeDamage(float damageAmount, float armourPenetration)
    {
        // armour reduction
        float damageTaken = GameController.Instance.armourClass.armourDeductionBase(statsCopy.armour, armourPenetration, damageAmount);

        if (damageTaken > 0)
        {
            // play audio
            AudioManager.Instance.PlayAudioClip(AudioKey.EnemyDamaged);

            currentHealth -= damageTaken; // subtract health with normal (0-100)
            Debug.Log($"Damage taken is: {damageTaken}");

            healthBarFill.fillAmount = currentHealth / enemyData.maxHealth; // make sure fill scales with enemies total health

            if (currentHealth <= 0)
            {
                DropWeaponBox(); // drop weapon box
                Die(); // call Die function implemented by specific unit controller
            } 
        }
        else
        {
            // damage blocked, play audio for this...
            AudioManager.Instance.PlayAudioClip(AudioKey.BlockedDamage);
        }

        

        
    }

    // flat damage taken... meant for dots + other sources
    public void FlatTakeDamage(float amount)
    {
        currentHealth -= amount; // subtract health with normal (0-100)
        Debug.Log($"Damage taken is: {amount}");

        healthBarFill.fillAmount = currentHealth / enemyData.maxHealth; // set enemy units health bar fill amount

        if (currentHealth <= 0)
        {
            DropWeaponBox(); // drop weapon box
            Die(); // call Die function implemented by specific unit controller
        } 
    }

    private void DropWeaponBox()
    {
        // Random
        // Weapon Box drop on enemy death
        // roll 50% chance that a weapon box drops...
        if (UnityEngine.Random.value <= 0.5f)
        {
            GameController instance = GameController.Instance;
            // have unit drop a box...
            GameObject box = Instantiate(instance.weaponBox, transform.position, transform.rotation);
            WeaponName weaponName = instance.GetWeaponBoxDropName();
            Debug.Log($"weapon name for box drop is: {weaponName}");
            box.GetComponentInChildren<WeaponBox>().SetWeaponToBox(weaponName); // weaponName ID to the box that drops
        }
    }


    // Enemy UI Debuff bar
     // add debuff
    public void AddEffect(WeaponDebuffData data)
    {
        
        if (activeIcons.ContainsKey(data.debuffId)) return;

        var icon = Instantiate(data.overlayEffectPrefab, effectTray);
        activeIcons[data.debuffId] = icon; // key - debuffId / value - prefab icon
        Debug.Log($"Added player debuff ICON: {activeIcons}");
    }

    // remove debuff
    public void RemoveEffect(List<WeaponDebuffData> enemyEffects)
    {
        for(int i = 0; i <enemyEffects.Count; i++)
        {

            if (activeIcons.TryGetValue(enemyEffects[i].debuffId, out var icon))
            {
                activeIcons.Remove(enemyEffects[i].debuffId);
                Destroy(icon);
                enemyEffects.RemoveAt(i);
            }
        }
        Debug.Log($"Added player debuff ICON: {activeIcons}");
    }

   
    // attack hitbox visual
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * enemyData.attackRange);
        // Drawing a rough cone representation
        Gizmos.color = Color.yellow;
        Vector3 leftBoundary = Quaternion.Euler(0, 0, enemyData.attackAngle / 2) * transform.up;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, -enemyData.attackAngle / 2) * transform.up;
        Gizmos.DrawRay(transform.position, leftBoundary * enemyData.attackRange);
        Gizmos.DrawRay(transform.position, rightBoundary * enemyData.attackRange);
    }
}

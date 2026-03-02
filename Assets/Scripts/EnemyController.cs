using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


// if we want a dot, we just need lifetime..


// Applies a flat deduction on statsCopy based on lifetime...
[System.Serializable]
public class ActiveDebuff
{
    public WeaponDebuffData data;
    public StatsCopy debuffedStats;
    public float timeRemaining;

    public ActiveDebuff(WeaponDebuffData data, StatsCopy debuffedStats)
    {
        this.data = data;
        timeRemaining = data.lifetime;
        this.debuffedStats = debuffedStats;
    }
}


// Debuffs in mind
// 1. stun
// 2. movement speed slowed
// 3. DOT

// need to implement a debuff controller

// we return new stats on affected unit (enemy or player)
// lifetime on that effect
// some effect ID
// when effect ends we compare its statsCopy to original unit stats, and remove debuff by grabbing differential of debuff properties
    // cannot set back to original cause other debuffs would be affected

// 

[System.Serializable]
public class StatsCopy
{
    // These are the stats prone to modification via debuffs/buffs
    public float currentHealth;
    //  dots would just apply to actual enemy or player currentHealth but be controlled by debuff controller




    // public float maxHealth;
    // public float damage;
    public float moveSpeed;
    public float armour;
    // public float attackCooldown;

    // Constructor to clone the ScriptableObject data into this instance
    public StatsCopy(float currentHealth, float moveSpeed, float armour)
    {
        // maxHealth = data.maxHealth;
        this.currentHealth = currentHealth;
        // damage = data.damage;
        this.moveSpeed = moveSpeed;
        this.armour = armour;
        // attackCooldown = data.attackCooldown;
    }
}



// Base enemy implementation
public abstract class EnemyController : MonoBehaviour
{
    // Base Settings
    private Transform target;
    private Transform myTransform;
    public Transform healthBarTransform; // can set on enemy prefab...
    private Rigidbody2D rb;
    public EnemyData enemyData; // Drag your ShamblerData or BossData here

    private StatsCopy statsCopy; // copy of data to apply effects too...
    public List<ActiveDebuff> activeDebuffs = new List<ActiveDebuff>();

    // apply debuff to data, set a timer for that effect, in update check array for timers hitting zero, normalize 

    private float nextAttackTime;
    private bool isAttacking = false;

    private bool isAffected = false; // for special effects 

    // Unit Stats
    protected float currentHealth;

    // Enemy UI
    public GameObject healthBarPrefab;
    public Image healthBarFill;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        myTransform = transform; // assign own transform
        rb = GetComponent<Rigidbody2D>(); // get rigidbody
        target = GameObject.FindGameObjectWithTag("Player").transform; // get player transform to follow...
        currentHealth = enemyData.maxHealth; // set health...
        // healthBarPrefab.SetActive = false;

        // set enemy stats copy
        statsCopy = new StatsCopy(enemyData.maxHealth, enemyData.moveSpeed, enemyData.armour);

        // Canvas canvas = GetComponentInChildren<Canvas>();
        // canvas.worldCamera = Camera.main;
    }

    // Child class Implemented Methods
    protected abstract void Die(); // unit death
    protected abstract void ExecuteAttackLogic(); // units attack pattern

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enemy unit hit by something");
        // base environment projectile destruction
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Projectile")
        {
            // Debug.Log("Hit an environment Layer!");

            BaseProjectile projectile = collision.GetComponent<BaseProjectile>(); // grab projectiles damage

            TakeDamage(projectile.damage); // unit should lose health...

            // check if projetile / weapon has a special effect to apply...
            if(projectile.isSpecialEffect)
            {
                StartCoroutine(ApplySpecialEffectImpact(projectile));                
            }
        }
        // if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        // {
        //     Debug.Log("Enemy !!!");
        //     //EnemyHit(gameObject);
            
        // }
    }


    // special effect method
    // Coroutine to cause delay so special effects can influence unit movement
    private IEnumerator ApplySpecialEffectImpact(BaseProjectile projectile)
    {
        isAffected = true; 
        
        // Apply the knockback/effect and set statsCopy
        // projectile special effects can either debuff a unit or apply whatever logic at end of projectile
        DebuffController debuffController = projectile.SpecialEffect(gameObject, statsCopy, projectile);
        
        // statsCopy = debuffController.debuffedStats; // set debuffed stats...
        // apply timer for this particular debuff...
        // 2. Register the debuff into our tracking list
        AddDebuff(debuffController);

        // Wait for a short duration so the physics force actually moves the object
        yield return new WaitForSeconds(0.2f); 
        
        isAffected = false;
    }


    void Update()
    {
        if (target == null || isAttacking || isAffected) return; // no target 

        HandleRotation(); // rotate eneny
        
        // Check if we can attack
        if (Time.time >= nextAttackTime)
        {
            // Raycast check: You can also make the 'Detection' logic abstract 
            // if some enemies don't use raycasts to trigger attacks.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, enemyData.attackRange, LayerMask.GetMask("Player"));

            if (hit.collider != null)
            {
                Debug.Log($"Enemy is using its basic attack.... stage1");
                StartCoroutine(AttackSequence());
            }
            else
            {
                MoveTowardsPlayer();
            }
        }
        else
        {
            MoveTowardsPlayer();
        }

        // check debuff status
        HandleDebuffTimers();
    }

    // debuff stuff
    private void HandleDebuffTimers()
    {
        // Loop backwards so we can safely remove items while iterating
        for (int i = activeDebuffs.Count - 1; i >= 0; i--)
        {
            // check if debuff is a dot... if so, then we need to drop health 
            ActiveDebuff debuff = activeDebuffs[i];
        
            // before
            int secondsBefore = Mathf.FloorToInt(debuff.timeRemaining);

            debuff.timeRemaining -= Time.deltaTime;

            // after to check whole seconds...
            int secondsAfter = Mathf.FloorToInt(debuff.timeRemaining);

            // dot logic, and when whole seconds are hit. Damage should happen
            if (secondsAfter < secondsBefore && debuff.timeRemaining > 0)
            {
                if (debuff.data.isDot) // Assuming your data has this bool
                {
                    ApplyDotDamage(debuff.data.effectIntensity);
                }
            }

            // debuff should be removed
            if (debuff.timeRemaining <= 0)
            {
                if (debuff.data.isDot) // dots last tick should be when it expires
                {
                    ApplyDotDamage(debuff.data.effectIntensity);
                }
                RemoveDebuff(debuff.data, debuff.debuffedStats); // remove debuff from queue
                activeDebuffs.RemoveAt(i);
            }
            // debuff still good, check to reapply effects if better debuffs died
            else
            {
                CheckSimilarDebuffs(debuff.debuffedStats);
            }
            Debug.Log($"Debuffs in list: {activeDebuffs.Count}");
        }
    }

    public void AddDebuff(DebuffController data)
    {
        // Optional: Check if debuff already exists to refresh timer instead of stacking
        ActiveDebuff existing = activeDebuffs.Find(d => d.data.debuffId == data.debuffData.debuffId);
        if (existing != null)
        {
            existing.timeRemaining = data.debuffData.lifetime;
            return;
        }

        // lets stack debuff hehe
        activeDebuffs.Add(new ActiveDebuff(data.debuffData, data.debuffedStats));
        // Apply initial stat changes here if needed
        //statsCopy = data.debuffedStats; // this sets everything to just this single debuff stats

        // Need to apply debuff properties to statsCopy in piece meal
        // if normal moveSpeed does not equal debuff, we apply
        // 70% moveSpeed + 50% moveSpeed
        CheckSimilarDebuffs(data.debuffedStats);


        // Debug.Log($"Debuff move speed is {data.debuffedStats.moveSpeed}...........");
        // Debug.Log($"ORIGINAL move speed is {enemyData.moveSpeed}...........");

    }

    private void CheckSimilarDebuffs(StatsCopy debuffedStats)
    {
        if (enemyData.moveSpeed != debuffedStats.moveSpeed)
        {
            // apply moveSpeed debuff to statsCopy
            // apply higher percent debuff
            if(statsCopy.moveSpeed > debuffedStats.moveSpeed)
            {
                statsCopy.moveSpeed = debuffedStats.moveSpeed; // apply worse debuff...
            }
        }

        // check armour too
        if (enemyData.armour != debuffedStats.armour)
        {
            // apply armour debuff to statsCopy
            if(statsCopy.armour > debuffedStats.armour)
            {
                statsCopy.armour = debuffedStats.armour; // apply worse debuff...
            }
        }
    }


    private void RemoveDebuff(WeaponDebuffData data,StatsCopy debuffedStats)
    {
        Debug.Log($"Debuff {data.debuffId} expired.");
        // Logic to revert statsCopy back to original values
        statsCopy.moveSpeed = enemyData.moveSpeed;
        statsCopy.armour = enemyData.armour;

        // apply lesser debuffs when larger ones are removed...
        //CheckSimilarDebuffs(debuffedStats);
    }

    // apply dot damage...
    private void ApplyDotDamage(float dotPercent)
    {
        currentHealth -= enemyData.maxHealth * dotPercent; // subtract dot damage from health... 
    }



    private IEnumerator AttackSequence()
    {
        isAttacking = true;
        
        // 1. Wind up - Every enemy pauses briefly
        yield return new WaitForSeconds(enemyData.windUpTime);

        // 
        ExecuteAttackLogic();

        // 3. Recovery / Cooldown
        yield return new WaitForSeconds(enemyData.attackCooldown);
        
        nextAttackTime = Time.time + enemyData.attackCooldown;
        isAttacking = false;
    }

    

    // receive flat damage...
    public void TakeDamage(float amount)
    {
        currentHealth -= amount; // subtract health with normal (0-100)
        Debug.Log($"Damage taken is: {amount}");

        healthBarFill.fillAmount = currentHealth / 100; // set enemy units health bar fill amount

        if (currentHealth <= 0)
        {
            // Weapon Box drop on enemy death
            // roll 50% chance that a weapon box drops...
            if (Random.value <= 0.5f)
            {
                GameController instance = GameController.Instance;
                // have unit drop a box...
                GameObject box = Instantiate(instance.weaponBox, transform.position, transform.rotation);
                WeaponName weaponName = instance.GetWeaponBoxDropName();
                Debug.Log($"weapon name for box drop is: {weaponName}");
                box.GetComponentInChildren<WeaponBox>().SetWeaponToBox(weaponName); // weaponName ID to the box that drops
            }
            
            // unit dies...
            Die(); // call Die function implemented by specific unit controller
        } 
    }


    
    // Movement rotation
    void HandleRotation()
    {
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    // Movement position
    void MoveTowardsPlayer()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        //Vector2 newPosition = Vector2.MoveTowards(rb.position, target.position, enemyData.moveSpeed * Time.deltaTime);
        // rb.MovePosition(newPosition);
        rb.linearVelocity = direction * statsCopy.moveSpeed;
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

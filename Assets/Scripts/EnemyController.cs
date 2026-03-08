// using System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

// Base enemy implementation
public abstract class EnemyController : MonoBehaviour
{
    // Base Settings
    private Transform target; // player transform
    private Transform myTransform;
    public Transform healthBarTransform; // can set on enemy prefab...
    private Rigidbody2D rb;
    public EnemyData enemyData; // Drag your ShamblerData or BossData here

    private StatsCopy statsCopy; // copy of data to apply effects too...
    // public List<ActiveDebuff> activeDebuffs = new List<ActiveDebuff>();

    private DebuffController enemyDebuffController;

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

        enemyDebuffController = new DebuffController(statsCopy); // debuff controller for enemy unit

        // Canvas canvas = GetComponentInChildren<Canvas>();
        // canvas.worldCamera = Camera.main;
    }

    // Child class Implemented Methods
    protected abstract void Die(); // unit death
    //protected abstract void ExecuteAttackLogic(); // units attack pattern

    // protected abstract void SpecialHitBoxAttackEffect(GameObject player, StatsCopy playerStats); // special effect on enemy hitbox attack
    //protected abstract void SpecialProjectileEffect(GameObject player, StatsCopy playerStats, BaseProjectile projectile); // special 

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
                StartCoroutine(ApplySpecialEffectProjectile(projectile));                
            }
        }
        // if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        // {
        //     Debug.Log("Enemy !!!");
        //     //EnemyHit(gameObject);
            
        // }
    }

    // Projectile special effect ONTO enemy unit
    // special effect method
    // Coroutine to cause delay so special effects can influence unit movement
    private IEnumerator ApplySpecialEffectProjectile(BaseProjectile projectile)
    {
        isAffected = true; 
        
        // Apply the knockback/effect and set statsCopy
        // projectile special effects can either debuff a unit or apply whatever logic at end of projectile
        Tuple<WeaponDebuffData, StatsCopy> debuffTuple = projectile.SpecialEffect(gameObject, statsCopy, projectile);

        // Tuple<WeaponDebuffData, StatsCopy> debuffTuple = attackHitboxScript.HitBoxSpecialEffect(gameObject, statsCopy);

        WeaponDebuffData data = debuffTuple.Item1;
        StatsCopy stats = debuffTuple.Item2;
        enemyDebuffController.SetDebuff(data,stats);
        
        // statsCopy = debuffController.debuffedStats; // set debuffed stats...
        // apply timer for this particular debuff...
        // 2. Register the debuff into our tracking list
        //AddDebuff(debuffController);

        // Wait for a short duration so the physics force actually moves the object
        yield return new WaitForSeconds(0.2f); 
        
        isAffected = false;
    }


    

    private IEnumerator AttackSequence()
    {
        isAttacking = true;
        
        // 1. Wind up - Every enemy pauses briefly
        yield return new WaitForSeconds(enemyData.windUpTime);

        // 
        //ExecuteAttackLogic();
        enemyData.AttackController(transform); // call unit attack controller..

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
            
            // unit dies...
            Die(); // call Die function implemented by specific unit controller
        } 
    }



    // Update lifetime 
    void Update()
    {
        if (target == null || isAttacking || isAffected) return; // no target 

        // DefaultHandleRotation(); // rotate eneny
        enemyData.DefaultRotation(target,transform);
        
        // Check if we can attack
        if (Time.time >= nextAttackTime)
        {

            // detection logic

            // attack sequence logic

            // 
            // if some enemies don't use raycasts to trigger attacks.
            // RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, enemyData.attackRange, LayerMask.GetMask("Player"));
            RaycastHit2D hit = enemyData.DefaultDetection(transform,enemyData);

            if (hit.collider != null)
            {
                Debug.Log($"Enemy is using its basic attack.... stage1");
                StartCoroutine(AttackSequence());
            }
            else
            {
                // DefaultMoveTowardsPlayer();
                enemyData.DefaultMovement(target,transform,rb,statsCopy.moveSpeed);
            }
        }
        else
        {
            // DefaultMoveTowardsPlayer();
            enemyData.DefaultMovement(target,transform,rb,statsCopy.moveSpeed);
        }

        // check debuff status
        // HandleDebuffTimers(); 
        statsCopy = enemyDebuffController.HandleDebuffTimers(); // set stats on debuff timer
        TakeDamage(enemyDebuffController.HandleDotTimers()); // apply dot damage
    }


    
    // Movement rotation
    // protected void DefaultHandleRotation()
    // {
    //     Vector3 direction = target.position - transform.position;
    //     float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //     transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    // }

    // Movement position
    // protected void DefaultMoveTowardsPlayer()
    // {
    //     Vector2 direction = (target.position - transform.position).normalized;
    //     //Vector2 newPosition = Vector2.MoveTowards(rb.position, target.position, enemyData.moveSpeed * Time.deltaTime);
    //     // rb.MovePosition(newPosition);
    //     rb.linearVelocity = direction * statsCopy.moveSpeed;
    // }

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

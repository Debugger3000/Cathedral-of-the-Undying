using System.Collections;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public class EnemyStatsCopy
{
    // These are the stats prone to modification via debuffs/buffs
    public float currentHealth;
    public float maxHealth;
    public float damage;
    public float moveSpeed;
    public float armour;
    public float attackCooldown;

    // Constructor to clone the ScriptableObject data into this instance
    public EnemyStatsCopy(EnemyData data)
    {
        maxHealth = data.maxHealth;
        currentHealth = data.maxHealth;
        damage = data.damage;
        moveSpeed = data.moveSpeed;
        armour = data.armour;
        attackCooldown = data.attackCooldown;
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

    private EnemyStatsCopy enemyStatsCopy; // copy of data to apply effects too...

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
        enemyStatsCopy = new EnemyStatsCopy(enemyData);

        // Canvas canvas = GetComponentInChildren<Canvas>();
        // canvas.worldCamera = Camera.main;
    }

    // implemented by actual unit script...
    protected abstract void Die(); // unit death

    // Subclasses MUST implement this (Cone, Circle, Projectile, etc.)
    protected abstract void ExecuteAttackLogic();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enemy unit hit by something");
        // base environment projectile destruction
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Projectile")
        {
            Debug.Log("Hit an environment Layer!");

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


    // New Coroutine to handle the pause in movement
    private IEnumerator ApplySpecialEffectImpact(BaseProjectile projectile)
    {
        isAffected = true; 
        
        // Apply the knockback/effect
        enemyStatsCopy = projectile.SpecialEffect(gameObject, enemyStatsCopy, projectile);
        
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
        rb.linearVelocity = direction * enemyStatsCopy.moveSpeed;
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

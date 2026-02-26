using System.Collections;
using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Base enemy implementation
public abstract class EnemyController : MonoBehaviour
{
    // Base Settings
    private Transform target;
    private Transform myTransform;
    public Transform healthBarTransform; // can set on enemy prefab...
    private Rigidbody2D rb;
    public EnemyData enemyData; // Drag your ShamblerData or BossData here

    private float nextAttackTime;
    private bool isAttacking = false;

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

        // Canvas canvas = GetComponentInChildren<Canvas>();
        // canvas.worldCamera = Camera.main;
    }

    // implemented by actual unit script...
    protected abstract void Die(); // unit death

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enemy unit hit by something");
        // base environment projectile destruction
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Projectile")
        {
            Debug.Log("Hit an environment Layer!");

            BaseProjectile projectile = collision.GetComponent<BaseProjectile>(); // grab projectiles damage

            TakeDamage(projectile.damage); // unit should lose health...
        }
        // if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        // {
        //     Debug.Log("Enemy !!!");
        //     //EnemyHit(gameObject);
            
        // }
    }

    void Update()
    {
        if (target == null || isAttacking) return;

        HandleRotation();
        
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
    }

    private IEnumerator AttackSequence()
    {
        isAttacking = true;
        
        // 1. Wind up - Every enemy pauses briefly
        yield return new WaitForSeconds(enemyData.windUpTime);

        // 2. THE ACTUAL ATTACK - This is now unique to each enemy
        ExecuteAttackLogic();

        // 3. Recovery / Cooldown
        yield return new WaitForSeconds(enemyData.attackCooldown);
        
        nextAttackTime = Time.time + enemyData.attackCooldown;
        isAttacking = false;
    }

    // Subclasses MUST implement this (Cone, Circle, Projectile, etc.)
    protected abstract void ExecuteAttackLogic();

    // receive flat damage...
    public void TakeDamage(float amount)
    {
        currentHealth -= amount; // subtract health with normal (0-100)
        Debug.Log($"Damage taken is: {amount}");

        healthBarFill.fillAmount = currentHealth / 100; //

        if (currentHealth <= 0) Die();
    }


    

    void HandleRotation()
    {
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    void MoveTowardsPlayer()
    {
        Vector2 newPosition = Vector2.MoveTowards(rb.position, target.position, enemyData.moveSpeed * Time.deltaTime);
        rb.MovePosition(newPosition);
    }

    // void CheckForAttack()
    // {
    //     if (Time.time >= nextAttackTime)
    //     {
    //         // Raycast forward to see if player is in front
    //         RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, enemyData.attackRange, LayerMask.GetMask("Player"));

    //         if (hit.collider != null)
    //         {
    //             StartCoroutine(PerformConeAttack()); // start delayed attack
    //         }
    //     }
    // }

    //
    // IEnumerator PerformConeAttack()
    // {
    //     isAttacking = true;
    //     nextAttackTime = Time.time + enemyData.attackCooldown;

    //     // 1. WIND UP: Warning phase (visual telegraph)
    //     Debug.Log("Enemy is charging attack...");
    //     yield return new WaitForSeconds(0.5f); // The "Dodge Window" for the player

    //     // 2. THE HIT: Check for player in cone
    //     Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, enemyData.attackRange, LayerMask.GetMask("Player"));

    //     foreach (var player in hitPlayers)
    //     {
    //         Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
    //         // Check if player is within the cone angle using Dot Product
    //         if (Vector3.Angle(transform.up, dirToPlayer) < enemyData.attackAngle / 2f)
    //         {
    //             Debug.Log("Player caught in cone attack!");
    //             // player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
    //         }
    //     }

    //     // 3. RECOVERY: Brief pause after swinging
    //     yield return new WaitForSeconds(0.3f);
    //     isAttacking = false;
    // }

    // Visualize the attack range in the Editor
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


    // Update 
    // void Update()
    // {
    //     //rotate to look at player
    //     Vector3 direction = target.position - myTransform.position;
    //     float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //     myTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));

    //     //move towards the player
    //     Vector3 newPosition = Vector3.MoveTowards(rb.position, target.position, enemyData.moveSpeed * Time.fixedDeltaTime);
    //     rb.MovePosition(newPosition);
    // }
}

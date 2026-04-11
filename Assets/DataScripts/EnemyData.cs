using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;




// [CreateAssetMenu(fileName = "NewWeaponData", menuName = "Guns/Weapon Data")]
public abstract class EnemyData : ScriptableObject
{
    [Header("General Stats")]
    public string enemyName; // enemy name...
    public float maxHealth = 100f; // max health
    // Armour
        // Armour should subtract flat from weapon damage
            // if armour makes weapon damage == 0, then we should buffer with a minimal percent of damage such as 5%... 
        // armour pen should subtract flat amount from armour
        // if Armour = 20
        // armour pen = 10 and damage is 15 
        // 20 - 10 = 10 armour --> 10 - 15 damage = gun does 5 damage ?
    public float armour = 10f;

    // shield
    // uses armour value. So basically weapons without enough pen won't do anything to the shield
    // public float shield = 100; // base shield health
    
    public float moveSpeed = 2f;
    public float rotationSpeed = 3f;

    [Header("Base Attack Stats")]
    public float damage = 10f; // enemy damage for hitbox
    public float armourPenetration = 1.0f;
    

    [Header("Projectile Attack Stats")]
    public float projectileMoveSpeed = 10f;
    public float projectileDamage = 10f; // projectile damage
    public bool isProjectileSpecialEffect = false;
    public WeaponDebuffData debuffProjectile;


    [Header("Hitbox Attack Stats")]
    public float attackRange = 2f;       // How far the raycast / detection goes
    public float windUpTime = 2f;       // Raycast detection to attack time
    public float attackCooldown = 1.5f;  // Time between attacks
    public float hitboxLifetime = 1.0f; // how long the hitbox visual should be shown
    public float attackAngle = 45f;
    public bool isHitBoxSpecialEffect = false;
    public WeaponDebuffData debuffDataHitBox;

    

    [Header("Prefabs")]
    public GameObject projectilePrefab; // projectile prefab for weapon
    public GameObject enemyPrefab; // enemy prefab

    public GameObject attackHitbox; // hitbox prefab visual

    public List<GameObject> attackHitboxList = new List<GameObject>(); // hitbox prefab visual

    public Queue<int> attackQueue = new(new[] { 0, 0, 0 });

    // Private Vars
    private float avoidanceDirection = 0f;
    private float avoidanceLockTimer = 0f;

    protected GameObject activeHitbox;
    public int attackIndex = 0; // attack index for DEMON lmao yikes
    // ----------------------
    // Movement / Rotation Functions
    // rotation can be overridden by enemy data
    public virtual void DefaultRotation(Transform target, Transform unitTransform, float rotationSpeed = 3f)
    {
        // rotating enemy face towards player transform
        Vector3 direction = target.position - unitTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        unitTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

    }

    // movement can be overridden
    public virtual void DefaultMovement(Transform target, Transform unitTransform, Rigidbody2D rb, float moveSpeed)
    {
        Vector2 direction = (target.position - unitTransform.position).normalized;
        //Vector2 newPosition = Vector2.MoveTowards(rb.position, target.position, enemyData.moveSpeed * Time.deltaTime);
        // rb.MovePosition(newPosition);
        rb.linearVelocity = direction * moveSpeed;
        
    }

    public virtual void AvoidanceMovement(Transform unitTransform, Rigidbody2D rb, float moveSpeed)
    {
        rb.linearVelocity = (Vector2)unitTransform.up * moveSpeed;
    }

    // RAYCAST detection for player
    public virtual RaycastHit2D DefaultDetection(Transform transform, EnemyData enemyData)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, enemyData.attackRange, LayerMask.GetMask("Player"));
        return hit;
    }

    // Environment detection
    public virtual (bool detected, float openAngle) EnvironmentDetection(Transform transform)
    {
        float rayDistance = 3f;
        int mask = LayerMask.GetMask("Environment");
        // Wide scan
        float[] angles = { 0f, 15f, -15f, 30f, -30f, 45f, -45f, 60f, -60f, 90f, -90f };

        List<float> blockedAngles = new List<float>();
        List<float> openAngles = new List<float>();

        foreach (float offset in angles)
        {
            Vector2 dir = Quaternion.Euler(0, 0, offset) * transform.up;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, rayDistance, mask);

            if (hit.collider != null)
                blockedAngles.Add(offset);
            else
                openAngles.Add(offset);
        }

        if (blockedAngles.Count == 0)
            return (false, 0f);

        if (openAngles.Count == 0)
        {
            // Everything blocked — just go backwards
            return (true, 180f);
        }

        // Pick an open angle — prefer closest to center for shortest path
        // But 30% chance to pick a random open angle for variety
        float chosenAngle;
        if (avoidanceLockTimer <= 0f)
        {
            if (Random.value < 0.7f)
            {
                // Closest open angle to center
                openAngles.Sort((a, b) => Mathf.Abs(a).CompareTo(Mathf.Abs(b)));
                chosenAngle = openAngles[0];
            }
            else
            {
                // Random open angle
                chosenAngle = openAngles[Random.Range(0, openAngles.Count)];
            }

            avoidanceDirection = chosenAngle;
            avoidanceLockTimer = Random.Range(0.8f, 1.5f);
        }
        else
        {
            avoidanceLockTimer -= Time.deltaTime;
            chosenAngle = avoidanceDirection;
        }

        return (true, chosenAngle);
    }

    public virtual void EnvironmentAvoidanceRotation(Transform target, Transform unitTransform, float openAngle)
    {
        // Rotate toward the open space
        Vector2 openDir = Quaternion.Euler(0, 0, openAngle) * unitTransform.up;

        // Blend slightly toward player so it doesn't wander forever
        Vector2 toPlayer = ((Vector2)target.position - (Vector2)unitTransform.position).normalized;
        Vector2 desired = (openDir * 2f + toPlayer * 0.5f).normalized;

        float angle = Mathf.Atan2(desired.y, desired.x) * Mathf.Rad2Deg;
        unitTransform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    //--------
    // Attack functions

    // Use this if to control attack sequencing for a enemy unit...
    public virtual string AttackController(Transform transform, Transform target, MonoBehaviour owner)
    {
        BasicHitBoxAttack(transform, target,owner); // perform basic hitbox attack
        return "attack";
    }

    public virtual void EnemyStopsAttack(Transform transform, Transform target, MonoBehaviour owner)
    {
        // stop attack

    }
    
    

    // Default hitbox basic attack
    // can use this for normal melee attacks
    public virtual void BasicHitBoxAttack(Transform transform, Transform target,MonoBehaviour owner)
    {
        //
       // Debug.Log("Enemy basic attack called...");
        // Debug.Log("Shambler performs a Cone Slam!");
        //Debug.Log($"Enemy is using its basic attack.... stage 2 hitbox instantiate");

        // 1. Get the rotation
        Quaternion spawnRotation = transform.rotation;

        // 2. Calculate a position slightly in front of the enemy face
        // 'transform.up' is the direction the enemy is facing. 
        // Multiply by 0.5f or 1.0f to push it out.
        Vector3 spawnPosition = transform.position + (transform.up * 1.5f);

        GameObject hitbox = Instantiate(attackHitbox, spawnPosition, spawnRotation); // generate hitbox

        hitbox.GetComponentInChildren<AttackHitboxController>().Setup(damage, armourPenetration, 0, hitboxLifetime, isHitBoxSpecialEffect, debuffDataHitBox);
        
        Destroy(hitbox,hitboxLifetime); // destroy hitbox after attack...
    }

    public virtual void BasicProjectileFire(Transform muzzle, Transform target)
    {

        GameObject bullet = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
            
            // Pass the stats from the gun to the projectile script
        BaseProjectile projScript = bullet.GetComponent<BaseProjectile>();
            // bullet.TryGetComponent(out BaseProjectile baseProjectileScript)
        if (projScript != null)
        {
            
            Debug.Log($"Enemyjust shot a PORJECTILE HAHAHAHAHAA");
            projScript.SetEnemyAttributes(projectileMoveSpeed, projectileDamage, armourPenetration, isProjectileSpecialEffect, debuffProjectile, target);
            // projScript.speed = currentWeaponData.weaponData.bulletSpeed;
            // projScript.damage = currentWeaponData.weaponData.weaponDamage;
        }
    }


    // Randomize
    // For demon yikes...
    public void RandomizeAttack()
    {
        for (int i = 0; i < 3; i++)
        {
            attackQueue.Enqueue(Random.Range(0, 2)); // 0 or 1
        }
    }
}


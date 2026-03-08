using UnityEngine;







// [CreateAssetMenu(fileName = "NewWeaponData", menuName = "Guns/Weapon Data")]
public abstract class EnemyData : ScriptableObject
{
    [Header("General Stats")]
    public string enemyName; // enemy name...
    public float maxHealth = 100f; // max health
    public float armour = 10f;
    // Armour
        // Armour should subtract flat from weapon damage
            // if armour makes weapon damage == 0, then we should buffer with a minimal percent of damage such as 5%... 
        // armour pen should subtract flat amount from armour
        // if Armour = 20
        // armour pen = 10 and damage is 15 
        // 20 - 10 = 10 armour --> 10 - 15 damage = gun does 5 damage ?
    public float moveSpeed = 2f;
    public float rotationSpeed = 3f;

    [Header("Attack Flags")]
    // public bool hasHitBoxAttack = false; // flag for what type of attack unit has
    // public bool hasProjectileAttack = false;
    public bool isHitBoxSpecialEffect = false;
    public bool isProjectileSpecialEffect = false;

    [Header("Attack Stats")]
    public float damage = 10f; // enemy damage for hitbox
    public float projectileDamage = 10f; // projectile damage
    public float projectileMoveSpeed = 10f;

    public float attackRange = 2f;       // How far the raycast / detection goes
    public float windUpTime = 2f;       // Raycast detection to attack time
    public float attackCooldown = 1.5f;  // Time between attacks
    public float hitboxLifetime = 1.0f; // how long the hitbox visual should be shown
    public float attackAngle = 45f;

    public WeaponDebuffData debuffDataHitBox;

    public WeaponDebuffData debuffProjectile;
    

    [Header("Prefabs")]
    public GameObject projectilePrefab; // projectile prefab for weapon
    public GameObject enemyPrefab; // enemy prefab

    public GameObject attackHitbox; // hitbox prefab visual


    // rotation can be overridden by enemy data
    public virtual void DefaultRotation(Transform target, Transform unitTransform)
    {
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

    // RAYCAST detection for player
    public virtual RaycastHit2D DefaultDetection(Transform transform, EnemyData enemyData)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, enemyData.attackRange, LayerMask.GetMask("Player"));
        return hit;
    }


    // Use this if to control attack sequencing for a enemy unit...
    public virtual void AttackController(Transform transform)
    {
        BasicHitBoxAttack(transform); // perform basic hitbox attack
    }

    // Default hitbox basic attack
    // can use this for normal melee attacks
    public virtual void BasicHitBoxAttack(Transform transform)
    {
        //
        Debug.Log("Enemy basic attack called...");
        // Debug.Log("Shambler performs a Cone Slam!");
        Debug.Log($"Enemy is using its basic attack.... stage 2 hitbox instantiate");

        // 1. Get the rotation
        Quaternion spawnRotation = transform.rotation;

        // 2. Calculate a position slightly in front of the enemy face
        // 'transform.up' is the direction the enemy is facing. 
        // Multiply by 0.5f or 1.0f to push it out.
        Vector3 spawnPosition = transform.position + (transform.up * 1.5f);

        GameObject hitbox = Instantiate(attackHitbox, spawnPosition, spawnRotation); // generate hitbox

        hitbox.GetComponentInChildren<AttackHitboxController>().Setup(damage, hitboxLifetime, isHitBoxSpecialEffect, debuffDataHitBox);
        
        Destroy(hitbox,1f); // destroy hitbox after attack...
    }

    public virtual void BasicProjectileFire(Transform muzzle)
    {

        GameObject bullet = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
            
            // Pass the stats from the gun to the projectile script
        BaseProjectile projScript = bullet.GetComponent<BaseProjectile>();
            // bullet.TryGetComponent(out BaseProjectile baseProjectileScript)
        if (projScript != null)
        {
            
            projScript.SetEnemyAttributes(projectileMoveSpeed, projectileDamage, isProjectileSpecialEffect, debuffProjectile);
            // projScript.speed = currentWeaponData.weaponData.bulletSpeed;
            // projScript.damage = currentWeaponData.weaponData.weaponDamage;
        }
        
    }


}


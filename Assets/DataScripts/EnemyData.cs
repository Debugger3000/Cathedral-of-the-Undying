using UnityEngine;







// [CreateAssetMenu(fileName = "NewWeaponData", menuName = "Guns/Weapon Data")]
public abstract class EnemyData : ScriptableObject
{
    public string enemyName; // enemy name...

    public float damage = 10f; // enemy damage

    public float attackRange = 2f;       // How far the raycast goes
    public float windUpTime = 2f;       // Raycast detection to attack time
    public float attackCooldown = 1.5f;  // Time between attacks
    public float hitboxLifetime = 1.0f; // how long the hitbox visual should be shown
    public float attackAngle = 45f;

    public WeaponDebuffData debuffDataHitBox;
    public bool isHitBoxSpecialEffect = false;

    public float maxHealth = 100f;

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
    
    
    // public GameObject projectilePrefab; // projectile prefab for weapon
    public GameObject enemyPrefab; // enemy prefab

    public GameObject attackHitbox; // hitbox prefab visual

    // probably dont need here
    public virtual void EnemyHitByPlayerAttack()
    {
        
    }

    // Default basic attack
    // can use this for normal melee attacks
    public virtual void BasicAttack(Transform transform)
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

    // public abstract void Fire(Transform muzzle);


}


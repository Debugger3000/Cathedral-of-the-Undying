using UnityEngine;

// [CreateAssetMenu(fileName = "NewWeaponData", menuName = "Guns/Weapon Data")]
public abstract class WeaponData : ScriptableObject
{
    [Header("General Stats")]
    public string weaponName; // weapon name...
    
    [Header("Projectile Stats")]
    // private Transform muzzlePoint;
    // public float muzzleOffset = 0.4f;
    public float fireRate = 0.5f; // fire rate
    public float bulletSpeed = 20f; // bullet speed
    public float weaponDamage = 10f; // weapons damage
    public int initialWeaponAmmo = 999; // weapons initial ammo... ?????
    public float weaponPenetration = 10f; // if we introduce enemy armour 
    public int weaponPoints = 10; // how much this weapons projectiles should increment the point multiplier...

    public bool isSpecialEffect = false; // weapon has special effect on projectiles or on hit
    public WeaponDebuffData debuffData; // weapon debuff effects...

    [Header("Hitbox Stats")]
    public float HitBoxDamage = 10f;
    public float hitboxLifetime = 1f;
    //public float attackCooldown = 1.5f;  // Time between attacks
    public WeaponDebuffData debuffDataHitBox;

    public bool isHitBoxSpecialEffect = false;


    [Header("Prefabs")]
    public GameObject attackHitbox; // hitbox prefab visual
    public GameObject projectilePrefab; // projectile prefab for weapon
    public GameObject weaponPrefab; // weapon prefab

    public Sprite weaponSprite; // weapon visual for UI

    // upgrade level 1 - 3
    //public int upgradeLevel = 1;



    public virtual void AttackController(Transform playerTransform, Transform muzzleTransform)
    {
        FireProjectile(muzzleTransform); // default attack is projectile
    }

    // Default fire behaviour
    // One round shots...
    // effects will be put onto projectiles
    public virtual void FireProjectile(Transform muzzle)
    {
        //
            Debug.Log("Pew!");

            // set fire rate time for next fire
            // nextFireTime = Time.time + (1f / currentWeaponData.weaponData.fireRate);

            // Instantiate the specific projectile for THIS gun
            GameObject bullet = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
            
            // Pass the stats from the gun to the projectile script
            BaseProjectile projScript = bullet.GetComponent<BaseProjectile>();
            // bullet.TryGetComponent(out BaseProjectile baseProjectileScript)
            if (projScript != null)
            {
                
                projScript.SetAttributes(bulletSpeed, weaponDamage, weaponPoints, isSpecialEffect, debuffData);
                // projScript.speed = currentWeaponData.weaponData.bulletSpeed;
                // projScript.damage = currentWeaponData.weaponData.weaponDamage;
            }
    }

    public virtual void HitBoxAttack(Transform playerTransform)
    {
        //
        Debug.Log("Enemy basic attack called...");
        // Debug.Log("Shambler performs a Cone Slam!");
        Debug.Log($"Enemy is using its basic attack.... stage 2 hitbox instantiate");

        // 1. Get the rotation
        Quaternion spawnRotation = playerTransform.rotation;

        // 2. Calculate a position slightly in front of the enemy face
        // 'transform.up' is the direction the enemy is facing. 
        // Multiply by 0.5f or 1.0f to push it out.
        Vector3 spawnPosition = playerTransform.position + (playerTransform.up * 1.5f);

        GameObject hitbox = Instantiate(attackHitbox, spawnPosition, spawnRotation); // generate hitbox

        hitbox.GetComponentInChildren<AttackHitboxController>().Setup(HitBoxDamage, hitboxLifetime, isHitBoxSpecialEffect, debuffDataHitBox);
        
        Destroy(hitbox,1f); // destroy hitbox after attack...
    }


}

using System.Collections.Generic;
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
    public float armourPenetration = 10f; // if we introduce enemy armour 
    public int weaponPoints = 10; // how much this weapons projectiles should increment the point multiplier...

    public bool isSpecialEffect = false; // weapon has special effect on projectiles or on hit
    public WeaponDebuffData debuffData; // weapon debuff effects...

    [Header("Heat Settings")]
    public bool hasOverHeat = false;
    public float heatPerShot = 0.1f;    // 10 shots to overheat
    public float heatCoolDownRate = 0.5f;   // How fast heat drops per second
    public float overHeatCoolDown = 3f; // once overheated, how long to fire...
    
    

    [Header("Hitbox Stats")]
    public float HitBoxDamage = 10f;
    public float hitboxLifetime = 1f;
    //public float attackCooldown = 1.5f;  // Time between attacks
    public WeaponDebuffData debuffDataHitBox;

    public bool isHitBoxSpecialEffect = false;


    [Header("Prefabs")]
    public GameObject attackHitbox; // hitbox prefab visual
    public GameObject projectilePrefab; // projectile prefab for weapon
    public List<GameObject> childrenProjectiles; // children projectiles, if any...
    public GameObject weaponPrefab; // weapon prefab

    public Sprite weaponSprite; // weapon visual for UI

     [Header("Distance Effect")]
    public bool hasDistanceEffect = false;
    public float distanceForDistanceEffect = 5f;


    protected GameObject activeHitbox;

    // upgrade level 1 - 3
    //public int upgradeLevel = 1;



    public virtual void AttackController(Transform playerTransform, Transform muzzleTransform)
    {
        FireProjectile(muzzleTransform); // default attack is projectile
    }


    // Flamethrower implementation basically... Stop a hitbox from existing...
    public virtual void StopAttackController(Transform playerTransform, Transform muzzleTransform)
    {
        if (activeHitbox != null)
        {
             Debug.Log($"STOPPING FLAMER ATTACK HITBOX.//...............");
            Destroy(activeHitbox);
            activeHitbox = null;
             Debug.Log($"HIBOX: {activeHitbox}");
        }
    }

    protected abstract void FireProjectileSound();

    // Default fire behaviour
    // One round shots...
    // effects will be put onto projectiles
    public virtual void FireProjectile(Transform muzzle)
    {
        //
            //Debug.Log("Pew!");
            FireProjectileSound(); // sound of weapon...

            // set fire rate time for next fire
            // nextFireTime = Time.time + (1f / currentWeaponData.weaponData.fireRate);

            // Instantiate the specific projectile for THIS gun
            GameObject bullet = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
            
            // Pass the stats from the gun to the projectile script
            BaseProjectile projScript = bullet.GetComponent<BaseProjectile>();
            // bullet.TryGetComponent(out BaseProjectile baseProjectileScript)
            if (projScript != null)
            {
                
                projScript.SetAttributes(bulletSpeed, weaponDamage, armourPenetration, weaponPoints, isSpecialEffect, debuffData, hasDistanceEffect, distanceForDistanceEffect, childrenProjectiles);
                // projScript.speed = currentWeaponData.weaponData.bulletSpeed;
                // projScript.damage = currentWeaponData.weaponData.weaponDamage;
            }
    }

    

    public virtual void HitBoxAttack(Transform playerTransform, Transform muzzle)
    {
        //
        //Debug.Log("Enemy basic attack called...");
        Debug.Log($"Hitbox Attack Player: {muzzle} { playerTransform}");
        Debug.Log($"Muzzle pos: {muzzle.position} rot: {muzzle.rotation} scale: {muzzle.lossyScale}");

        

        // Get the rotation
        //Quaternion spawnRotation = playerTransform.rotation;

        // Calculate a position slightly in front of the enemy face
        // 'transform.up' is the direction the enemy is facing. 
        // Multiply by 0.5f or 1.0f to push it out.
        Vector3 spawnPosition = muzzle.position + (muzzle.up * 2f);
        Debug.Log($"Spawn pos: {spawnPosition}");

        GameObject hitbox = Instantiate(attackHitbox, spawnPosition, muzzle.rotation); // generate hitbox

        hitbox.GetComponentInChildren<AttackHitboxController>().Setup(HitBoxDamage, armourPenetration, weaponPoints, hitboxLifetime, isHitBoxSpecialEffect, debuffDataHitBox);
        Debug.Log($"After hitvox created.........");
        Destroy(hitbox,1f); // destroy hitbox after attack...
    }


}

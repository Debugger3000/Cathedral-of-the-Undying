using UnityEngine;

// [CreateAssetMenu(fileName = "NewWeaponData", menuName = "Guns/Weapon Data")]
public abstract class EnemyData : ScriptableObject
{
    public string enemyName; // weapon name...

    public float damage = 10f; // weapons damage

    public float attackRange = 2f;       // How far the raycast goes
    public float windUpTime = 2f;       // Raycast detection to attack time
    public float attackCooldown = 1.5f;  // Time between attacks
    public float hitboxLifetime = 1.0f; // how long the hitbox visual should be shown
    public float attackAngle = 45f;

    public float maxHealth = 100f;

    public float armour = 10f;
    public float moveSpeed = 2f;
    public float rotationSpeed = 3f;
    
    
    // public GameObject projectilePrefab; // projectile prefab for weapon
    public GameObject enemyPrefab; // weapon prefab

    public GameObject attackHitbox;

    //public Sprite weaponSprite; // weapon visual for UI
    // private Transform muzzlePoint;
    // public float muzzleOffset = 0.4f;


    //public float fireRate = 0.5f; // fire rate
    //public float bulletSpeed = 20f; // bullet speed
    
    //public int initialWeaponAmmo = 999; // weapons initial ammo... ?????

    //public float weaponPenetration = 10f; // if we introduce enemy armour 

    // upgrade level 1 - 3
    //public int upgradeLevel = 1;


    public virtual void EnemyHitByPlayerAttack()
    {
        
    }

    // Default basic attack
    // can use this for normal melee attacks
    public virtual void BasicAttack(Transform muzzle)
    {
        //
            Debug.Log("Enemy basic attack called...");

            // basic enemy attack
            // small raycasted aoe cone infront of themselves when they collide or get close to player...




            // set fire rate time for next fire
            // nextFireTime = Time.time + (1f / currentWeaponData.weaponData.fireRate);

            // Instantiate the specific projectile for THIS gun
            //GameObject bullet = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
            
            // Pass the stats from the gun to the projectile script
            //BaseProjectile projScript = bullet.GetComponent<BaseProjectile>();
            // bullet.TryGetComponent(out BaseProjectile baseProjectileScript)
            // if (projScript != null)
            // {
            //     //projScript.SetAttributes(bulletSpeed, weaponDamage);
            //     // projScript.speed = currentWeaponData.weaponData.bulletSpeed;
            //     // projScript.damage = currentWeaponData.weaponData.weaponDamage;
            // }
    }

    // public abstract void Fire(Transform muzzle);


    // pistol - level 1
    // pistol - level 2
    // pistol - level 3


}


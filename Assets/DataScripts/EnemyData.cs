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

    public GameObject attackHitbox;

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


}


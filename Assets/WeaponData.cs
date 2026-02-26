using UnityEngine;

// [CreateAssetMenu(fileName = "NewWeaponData", menuName = "Guns/Weapon Data")]
public abstract class WeaponData : ScriptableObject
{
    public string weaponName; // weapon name...
    public GameObject projectilePrefab; // projectile prefab for weapon
    public GameObject weaponPrefab; // weapon prefab

    public Sprite weaponSprite; // weapon visual for UI
    // private Transform muzzlePoint;
    // public float muzzleOffset = 0.4f;
    public float fireRate = 0.5f; // fire rate
    public float bulletSpeed = 20f; // bullet speed
    public float weaponDamage = 10f; // weapons damage
    public int initialWeaponAmmo = 999; // weapons initial ammo... ?????

    public float weaponPenetration = 10f; // if we introduce enemy armour 

    public int weaponPoints = 10; // how much this weapons projectiles should increment the point multiplier...

    // upgrade level 1 - 3
    //public int upgradeLevel = 1;

    // Default fire behaviour
    // One round shots...
    // effects will be put onto projectiles
    public virtual void Fire(Transform muzzle)
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
                projScript.SetAttributes(bulletSpeed, weaponDamage, weaponPoints);
                // projScript.speed = currentWeaponData.weaponData.bulletSpeed;
                // projScript.damage = currentWeaponData.weaponData.weaponDamage;
            }
    }

    // public abstract void Fire(Transform muzzle);


    // pistol - level 1
    // pistol - level 2
    // pistol - level 3


}

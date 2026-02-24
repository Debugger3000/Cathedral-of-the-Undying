using UnityEngine;

public class Weapon : BaseWeapon
{
    // public Transform muzzlePoint;

    // private WeaponInstance currentWeaponData;

    private float nextFireTime = 0f;

    void Awake()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }

    

    // public void Equip(WeaponInstance newData)
    // {
    //     if (newData == null) return;

    //     // 1. Destroy the old weapon model so they don't stack up
    //     foreach (Transform child in transform) 
    //     {
    //         // We destroy children so the "Hand" becomes empty
    //         Destroy(child.gameObject);
    //     }

    //     currentWeaponData = newData; // load new weapon data
    //     //weaponPrefab = newData.weaponPrefab; // load new weapon prefab
    //     //projectilePrefab = newData.projectilePrefab; // load new projectile prefab

    //     // insantiate new gun prefab, and hold object so we can grab muzzle point
    //     GameObject newGun = Instantiate(newData.weaponData.weaponPrefab, transform.position, transform.rotation, transform);
    //     // assign muzzle position
    //     muzzlePoint = newGun.transform.Find("Muzzle");
    //     // muzzlePoint = newGun.transform.Find("Muzzle");aw

    //     Debug.Log("Now holding: " + newData.weaponData.weaponName);
    // }

    public override void Fire()
    {
        // check firerate
        if (Time.time >= nextFireTime)
        {

            // currentWeaponData.weaponData.DefaultFire(muzzlePoint);
            WeaponInstance weapon = GetWeaponInstance();
            weapon.weaponData.Fire(muzzlePoint);
            //
            // Debug.Log("Pew!");

            // // set fire rate time for next fire
            nextFireTime = Time.time + (1f / weapon.weaponData.fireRate);

            // // Instantiate the specific projectile for THIS gun
            // GameObject bullet = Instantiate(currentWeaponData.weaponData.projectilePrefab, muzzlePoint.position, muzzlePoint.rotation);
            
            // // Pass the stats from the gun to the projectile script
            // BaseProjectile projScript = bullet.GetComponent<BaseProjectile>();
            // // bullet.TryGetComponent(out BaseProjectile baseProjectileScript)
            // if (projScript != null)
            // {
            //     projScript.SetAttributes(currentWeaponData.weaponData.bulletSpeed, currentWeaponData.weaponData.weaponDamage);
            //     // projScript.speed = currentWeaponData.weaponData.bulletSpeed;
            //     // projScript.damage = currentWeaponData.weaponData.weaponDamage;
            // }
        }
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}

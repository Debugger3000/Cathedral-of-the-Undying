using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Weapons/Shotgun")]
public class ShotgunData : WeaponData 
{
    [Header("Shotgun Settings")]
    // public GameObject pelletPrefab;
    public int pelletCount = 5;
    public float spreadAngle = 15f;
    // public float fireRate = 0.5f;

    public override void Fire(Transform muzzle) 
    {

        // GameObject bullet = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
            
        //     // Pass the stats from the gun to the projectile script
        //     BaseProjectile projScript = bullet.GetComponent<BaseProjectile>();
        //     // bullet.TryGetComponent(out BaseProjectile baseProjectileScript)
        //     if (projScript != null)
        //     {
        //         projScript.SetAttributes(bulletSpeed, weaponDamage);
        //         // projScript.speed = currentWeaponData.weaponData.bulletSpeed;
        //         // projScript.damage = currentWeaponData.weaponData.weaponDamage;
        //     }

        // Now you have access to EVERYTHING
        for (int i = 0; i < pelletCount; i++)
        {
            // Calculate a random rotation for the spread
            Quaternion spread = Quaternion.Euler(0, 0, Random.Range(-spreadAngle, spreadAngle));
            Quaternion finalRotation = muzzle.rotation * spread;

            // Spawn the pellet
            GameObject bullet = Instantiate(projectilePrefab, muzzle.position, finalRotation);

            BaseProjectile projScript = bullet.GetComponent<BaseProjectile>();
            // bullet.TryGetComponent(out BaseProjectile baseProjectileScript)
            if (projScript != null)
            {
                projScript.SetAttributes(bulletSpeed, weaponDamage);
                // projScript.speed = currentWeaponData.weaponData.bulletSpeed;
                // projScript.damage = currentWeaponData.weaponData.weaponDamage;
            }
        }

        Debug.Log($"Fired {pelletCount} pellets from {muzzle.name}");
    }
}
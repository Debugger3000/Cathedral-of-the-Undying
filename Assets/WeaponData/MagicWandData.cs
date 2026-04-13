using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Weapons/MagicWand")]
public class MagicWandData : WeaponData
{
    public float spreadAngle = 15f;
    public int splitCount = 3;

    protected override void FireProjectileSound()
    {
        AudioManager.Instance.PlayAudioClip(AudioKey.MagicWandFire);
    }

    public override void FireProjectile(Transform muzzle) 
    {
            // Spawn the pellet
            GameObject bullet = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
            BaseProjectile projScript = bullet.GetComponent<BaseProjectile>();

            // set special effect for this weapons projectile...
            MagicWandProjectile magicWandScript = bullet.GetComponent<MagicWandProjectile>();
            magicWandScript.splitCount = splitCount; // set split for projectile
            magicWandScript.spreadAngle = spreadAngle; // angle of spread

            if (projScript != null)
            {
                projScript.SetAttributes(bulletSpeed, weaponDamage, armourPenetration, weaponPoints, isSpecialEffect, debuffData, hasDistanceEffect, distanceForDistanceEffect, childrenProjectiles);
            }
        FireProjectileSound();
    }
}

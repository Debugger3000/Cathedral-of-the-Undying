using UnityEngine;


[CreateAssetMenu(fileName = "Weapons", menuName = "Weapons/Orb")]
public class OrbData : WeaponData 
{
    //[Header("Settings")]
    // public GameObject pelletPrefab;
    // public float effectIntensity = 1.00f; // 100% slow
    // public float effectDuration = 2f; // duration of effect...

    //public float knockbackForce = 5f; // knockback force on rb impulse
    // public float fireRate = 0.5f;

    protected override void FireProjectileSound()
    {
        AudioManager.Instance.PlayAudioClip(AudioKey.PlayerFiresProjectileWeapon);
    }

    public override void AttackController(Transform playerTransform, Transform muzzleTransform) 
    {
        HitBoxAttack(playerTransform); // perform hitbox attack instead
    }
}

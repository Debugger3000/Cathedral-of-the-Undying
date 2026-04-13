using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Weapons/Flamer")]
public class FlamerData : WeaponData
{
    [Header("Flamer Variables")]
    public float movementSpeedDebuff = 0.2f; // movement speed debuff while weapon is active on player...

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void FireProjectileSound()
    {
        AudioManager.Instance.PlayAudioClip(AudioKey.FlamerFire);
    }

    // override since default is a projectile
    // implement hitbox based attack
    public override void AttackController(Transform playerTransform, Transform muzzleTransform) 
    {
        HitBoxAttack(playerTransform, muzzleTransform); // perform hitbox attack instead
    }


    
    // OVERRIDE default hitboxattack for flamer functionality.
    // Flame has to follow muzzle point...
    public override void HitBoxAttack(Transform playerTransform, Transform muzzle) 
    {
        if (activeHitbox != null) return; // make sur ewe only instanitate one hitbox
        //
        //Debug.Log("Enemy basic attack called...");
        Debug.Log($"Hitbox Attack Player: {muzzle} { playerTransform}");
        // Parent to muzzle — it now moves/rotates with the player automatically

        // Get the rotation
        //Quaternion spawnRotation = playerTransform.rotation;

        // Calculate a position slightly in front of the enemy face
        // 'transform.up' is the direction the enemy is facing. 
        // Multiply by 0.5f or 1.0f to push it out.
        Vector3 spawnPosition = muzzle.position + (muzzle.up * 2.5f);

        activeHitbox = Instantiate(attackHitbox, spawnPosition, muzzle.rotation); // generate hitbox
        // Parent to muzzle — it now moves/rotates with the player automatically
        activeHitbox.transform.SetParent(muzzle);

        activeHitbox.GetComponentInChildren<AttackHitboxController>().Setup(HitBoxDamage, armourPenetration, weaponPoints, hitboxLifetime, isHitBoxSpecialEffect, debuffDataHitBox);
        Debug.Log($"After hitvox created.........");
        //Destroy(hitbox,hitboxLifetime); // destroy hitbox after attack...


    }

    
}

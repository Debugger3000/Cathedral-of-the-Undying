using UnityEngine;



[CreateAssetMenu(fileName = "Weapons", menuName = "Weapons/SniperRifle")]
public class SniperRifleData : WeaponData
{
    // gun shoot sound
    protected override void FireProjectileSound()
    {
        AudioManager.Instance.PlayAudioClip(AudioKey.PlayerFiresProjectileWeapon);
    }

}

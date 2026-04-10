using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Weapons/MagicWand")]
public class MagicWandData : WeaponData
{
    

    protected override void FireProjectileSound()
    {
        AudioManager.Instance.PlayAudioClip(AudioKey.PlayerFiresProjectileWeapon);
    }

    
}

using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Weapons/PlayerHeal")]
public class PlayerHealData : WeaponData
{
    // gun shoot sound
    protected override void FireProjectileSound()
    {
        AudioManager.Instance.PlayAudioClip(AudioKey.HealCratePickUp);
    }

}


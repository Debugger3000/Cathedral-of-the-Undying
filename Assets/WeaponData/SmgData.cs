using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Weapons/Smg")]
public class SmgData : WeaponData
{
    // gun shoot sound
    protected override void FireProjectileSound()
    {
        AudioManager.Instance.PlayAudioClip(AudioKey.SmgFire);
    }

}

using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponDebuff", menuName = "Weapons/WeaponDebuffData")]
public class WeaponDebuffData : ScriptableObject
{
    [Header("Identity")]
    public string debuffId; // Unique identifier (e.g., "slow_01")
    public string displayName;
    
    [TextArea(2, 5)]
    public string description;

    [Header("Settings")]
    public float lifetime; // How long the debuff lasts on the target
    // This needs to be a percent - 0.05 - 0.95
    public float effectIntensity; // expressed as %
    public bool isDot;

    public float damage; // damage is there is any

    
    // Add logic here later for visual effects (VFX) or Icons
    public GameObject overlayEffectPrefab;
}

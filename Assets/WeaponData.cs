using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Guns/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject projectilePrefab;
    public GameObject weaponPrefab;
    private Transform muzzlePoint;
    public float muzzleOffset = 0.4f;
    public float fireRate = 0.5f;
    public float bulletSpeed = 20f;
}

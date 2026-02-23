using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Guns/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject projectilePrefab;
    public GameObject weaponPrefab;
    // private Transform muzzlePoint;
    // public float muzzleOffset = 0.4f;
    public float fireRate = 0.5f;
    public float bulletSpeed = 20f;
    public float weaponDamage = 10f;
    public int initialWeaponAmmo = 999;

    // upgrade level 1 - 3
    //public int upgradeLevel = 1;


    // pistol - level 1
    // pistol - level 2
    // pistol - level 3


}

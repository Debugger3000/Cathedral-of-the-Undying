using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    private GameObject projectilePrefab;
    private GameObject weaponPrefab;
    public Transform muzzlePoint;

    private WeaponData currentWeaponData;
    // private SpriteRenderer weaponSprite;


    [Header("Weapon Stats")]
    public float fireRate = 0.2f;
    
    [Header("Projectile Stats")]
    public float bulletSpeed = 20f;
    public int damage = 10;

    void Awake()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Fire()
    {
        // Instantiate the specific projectile for THIS gun
        GameObject bullet = Instantiate(projectilePrefab, muzzlePoint.position, muzzlePoint.rotation);
        
        // Pass the stats from the gun to the projectile script
        Projectile projScript = bullet.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.speed = bulletSpeed;
            projScript.damage = damage;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Equip(WeaponData newData)
    {
        if (newData == null) return;

        // 1. Destroy the old weapon model so they don't stack up
        // foreach (Transform child in transform) 
        // {
        //     // We destroy children so the "Hand" becomes empty
        //     Destroy(child.gameObject);
        // }

        currentWeaponData = newData; // load new weapon data
        weaponPrefab = newData.weaponPrefab; // load new weapon prefab
        projectilePrefab = newData.projectilePrefab; // load new projectile prefab

        // insantiate new gun prefab, and hold object so we can grab muzzle point
        GameObject newGun = Instantiate(newData.weaponPrefab, transform.position, transform.rotation, transform);
        // assign muzzle position
        muzzlePoint = newGun.transform.Find("Muzzle");
        // muzzlePoint = newGun.transform.Find("Muzzle");aw

        Debug.Log("Now holding: " + newData.weaponName);
    }
}

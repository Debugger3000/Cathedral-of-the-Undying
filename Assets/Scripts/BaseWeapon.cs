using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{

    private float damage;

    // public WeaponData weaponData;

    public Transform muzzlePoint;

    private WeaponInstance currentWeaponData;

    private float nextFireTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // child weapon classes should implement there own type of firing...
    public abstract void Fire();

    void SetDamage()
    {
        damage = currentWeaponData.weaponData.weaponDamage;
    }


    public void Equip(WeaponInstance newData)
    {
        if (newData == null) return;

        // 1. Destroy the old weapon model so they don't stack up
        foreach (Transform child in transform) 
        {
            // We destroy children so the "Hand" becomes empty
            Destroy(child.gameObject);
        }

        currentWeaponData = newData; // load new weapon data

        // insantiate new gun prefab, and hold object so we can grab muzzle point
        GameObject newGun = Instantiate(newData.weaponData.weaponPrefab, transform.position, transform.rotation, transform);
        // assign muzzle position
        muzzlePoint = newGun.transform.Find("Muzzle");
        // muzzlePoint = newGun.transform.Find("Muzzle");aw

        // set damage
        SetDamage();

        Debug.Log("Now holding: " + newData.weaponData.weaponName);
    }

    public WeaponInstance GetWeaponInstance()
    {
        return currentWeaponData;
    }
}

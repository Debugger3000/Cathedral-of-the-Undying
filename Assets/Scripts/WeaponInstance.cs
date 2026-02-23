using UnityEngine;


// Data class wrapper for weapons
[System.Serializable]
public class WeaponInstance
{

    public WeaponData weaponData; // hold weapon data

    public int curAmmo;

    // other attributes that a weapon instance should hold... such as cooldown
    private bool isOnCooldown = false;

    public WeaponInstance(WeaponData weaponData)
    {
        curAmmo = weaponData.initialWeaponAmmo;
        this.weaponData = weaponData;
    }

    public void SetAmmo(int incrementAmount)
    {
        curAmmo += incrementAmount; // increment ammo
    }

    // public WeaponData GetWeaponData()
    // {
    //     return weaponData;
    // }

}

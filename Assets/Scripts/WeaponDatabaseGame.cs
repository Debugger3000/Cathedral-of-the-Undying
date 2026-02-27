using System;
using System.Collections.Generic;
using UnityEngine;
// using Unity.Mathematics;

// Collection of WeaponNames (basically their IDs)
public enum WeaponName {
    Pistol,
    Shotgun,
    SniperRifle,
    AutoPistol,
    MagicWand
}

// Class data structure for ALL weapons and their drop level in relation to points multiplayer
[System.Serializable]
public class WeaponDropEntry {
    public WeaponName weaponName; // This shows as a dropdown!
    [Range(0, 10)] 
    public int dropLevel;     // The associated point level
}



public class WeaponDatabaseGame : MonoBehaviour
{
    // start weapon
    // Pistol
    public WeaponData startWeapon;

    // base weapons
    // Shotgun, SniperRifle, AutoPistol, MagicWand
    public List<WeaponData> baseWeapons;

     // level 2 weapons
    // public WeaponDatabase levelTwoWeapons;

    // level 3 upgraded weapons
    // public WeaponDatabase levelThreeWeapons;

    public List<WeaponDropEntry> weaponConfigs = new List<WeaponDropEntry>();

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
        
    // }


    public WeaponName GetWeaponDrop(int level)
    {
        Debug.Log($"multiplier level is: {level}");
        List<WeaponDropEntry> availableWeaponDrops = new List<WeaponDropEntry>();
        // see which weapons are able to drop based on points multiplier
        foreach (WeaponDropEntry entry in weaponConfigs)
        {
            // if weapon is available... and its not a pistol...
            // point multiplier level must be equal or greater than a weapons dropLevel...
            //
            if (level >= entry.dropLevel && entry.weaponName != WeaponName.Pistol)
            {
                availableWeaponDrops.Add(entry);
            }
        }
        // int hee = Random
        // add to a list...
        int roll = UnityEngine.Random.Range(0, availableWeaponDrops.Count);
        Debug.Log($"roll is: {roll} and avaialble weapons count is: {availableWeaponDrops.Count}");

        // return weapon instance...
        return availableWeaponDrops[roll].weaponName;
    }

    public WeaponInstance GetStarterWeapon()
    {
        return new WeaponInstance(startWeapon);
    }

    public WeaponInstance GetShotgun()
    {
        return new WeaponInstance(GetWeaponByName(WeaponName.Shotgun));
    } 

    // return weapon instance pick up
    public WeaponInstance GetWeaponPickUp(WeaponName weaponName)
    {
        return new WeaponInstance(GetWeaponByName(weaponName));
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    public WeaponData GetWeaponByName(WeaponName name)
    {
        return baseWeapons.Find(w => w.weaponName == name.ToString());
    }
}

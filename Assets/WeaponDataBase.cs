using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Weapons/Database")]
public class WeaponDatabase : ScriptableObject
{
    // A list of all weapons in the game
    public List<WeaponData> allWeapons;

    // Helper function to find a weapon by its name
    public WeaponData GetWeaponByName(string name)
    {
        return allWeapons.Find(w => w.weaponName == name);
    }
}

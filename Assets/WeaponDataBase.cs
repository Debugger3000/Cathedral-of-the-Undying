using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Weapons/Database")]
public class WeaponDatabase : ScriptableObject
{
    // A list of all weapons in the game
    public List<WeaponData> weapons;

    // public WeaponData startWeapon;

    // Helper function to find a weapon by its name
    public WeaponData GetWeaponByName(string name)
    {
        return weapons.Find(w => w.weaponName == name);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    private List<WeaponInstance> inventoryWeapons = new List<WeaponInstance>();

    private int curWeaponIndex = 0; // start on first index - pistol

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public WeaponInstance SwapWeapons()
    {
        int nextIndex = curWeaponIndex+1;
        // grab next weapon
        if(nextIndex == inventoryWeapons.Count)
        {
            curWeaponIndex = 0; // set index to 0
            // grab first indexed weapon
            return inventoryWeapons[0];
        }
        else
        {
            curWeaponIndex = nextIndex; // set index to next
            return inventoryWeapons[nextIndex];
        }
    }

    // add new weapon to inventory...
    public void WeaponPickUp(WeaponInstance newWeapon)
    {
        inventoryWeapons.Add(newWeapon);
        Debug.Log($"weapon pick up in inventroy... {inventoryWeapons.Count} ......... { inventoryWeapons}");
    }

    // deal with weapon upgrade
    public void WeaponUpgrade(WeaponInstance upgradedWeapon)
    {
        // swap out lower leveled weapon with its new upgraded version
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

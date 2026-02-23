using UnityEngine;



public class WeaponDatabaseGame : MonoBehaviour
{
    // start weapon
    public WeaponData startWeapon;

    // base weapons
    public WeaponDatabase baseWeapons;

     // level 2 weapons
    public WeaponDatabase levelTwoWeapons;

    // level 3 upgraded weapons
    public WeaponDatabase levelThreeWeapons;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public WeaponInstance GetStarterWeapon()
    {
        return new WeaponInstance(startWeapon);
    } 

    public WeaponInstance GetShotgun()
    {
        return new WeaponInstance(baseWeapons.GetWeaponByName("Shotgun"));
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{

    private float damage;

    public WeaponData weaponData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void SetDamage()
    {
        damage = weaponData.weaponDamage;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

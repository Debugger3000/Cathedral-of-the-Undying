using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{

    private float damage;

    // public WeaponData weaponData;

    public Transform muzzlePoint;

    private WeaponInstance currentWeaponData;

    private float nextFireTime = 0f; // wepaon fire rate...

    // Weapon OverHeat vars
    private bool isOverheated = false;
    private float currentHeat = 0;

    // ammo amounts are just ignored...

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Update()
    {
        if(currentHeat > 0)
        {
            currentHeat -= currentWeaponData.weaponData.heatCoolDownRate * Time.deltaTime;
        }
        else
        {
            isOverheated = false;
        }
        GameController.Instance.uiManager.UpdateWeaponFireRate(GetFireRateNormalized()); // update firerate UI
        GameController.Instance.uiManager.UpdateWeaponOverHeat(GetHeatNormalized());
    }

    

    // Fire WRAPPER
    // Call default fire behaviour defined within WeaponData
    public void PlayerAttackWithWeapon(Transform playerTransform)
    {
        // fireRate / cooldown of attack
        if (Time.time >= nextFireTime && !isOverheated)
        {
            // call attack Controller 
            currentWeaponData.weaponData.AttackController(playerTransform,muzzlePoint);
            // set fire rate time / cooldown
            nextFireTime = Time.time + (1f / currentWeaponData.weaponData.fireRate);

            // check if gun has oveheat functionality
            if(currentWeaponData.weaponData.hasOverHeat)
            {
                currentHeat += currentWeaponData.weaponData.heatPerShot; // increment heat
                Debug.Log($"Weapon heat: {currentHeat}.................................");
            }
            
            // check if weapon has OverHeated...
            if(currentHeat >= 1f)
            {
                currentWeaponData.weaponData.StopAttackController(playerTransform,muzzlePoint);
                isOverheated = true;
                Debug.Log("Weapon has overheated...");
            }
        }
    }

    // default implementation
    // simply calls function from weaponData to stop active hitbox...
    public void PlayerStopsAttackWithWeapon(Transform playerTransform)
    {
        currentWeaponData.weaponData.StopAttackController(playerTransform,muzzlePoint);
    }


    public float GetFireRateNormalized()
    {
        if (currentWeaponData == null) return 1f;

        // calculate the total time gap required between shots (e.g., 0.1s for SMG)
        float totalInterval = 1f / currentWeaponData.weaponData.fireRate;

        // calculate how much time is left until we can fire again
        float timeRemaining = nextFireTime - Time.time;

        //timeRemaining is 0 or negative, we are ready to fire (return 1)
        if (timeRemaining <= 0) return 1f;

        // make sure the value never goes outside the 0.0 - 1.0 range
        return 1f - Mathf.Clamp01(timeRemaining / totalInterval);
    }

    public float GetHeatNormalized()
    {
        // Since currentHeat is already 0 at cold and 1 at overheat,
        // we just clamp it to be safe and return it.
        return Mathf.Clamp01(currentHeat);
    }

    
    void SetDamage()
    {
        damage = currentWeaponData.weaponData.weaponDamage;
    }


    public void Equip(WeaponInstance newData)
    {
        if (newData == null) return;

        // if 
        if(currentWeaponData != null)
        {
            // pass current heat levels to weapon isntance before it is swapped out
            currentWeaponData.currentHeatInstance = currentHeat;
            if (isOverheated)
            {
                currentWeaponData.isOverHeatedInstance = true;
            }
            // update UI

        }
        

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
        // reset fire Rate so weapons cannot be hot swapped...
        nextFireTime = Time.time + (1f / currentWeaponData.weaponData.fireRate);
        if (currentWeaponData.weaponData.hasOverHeat)
        {
            currentHeat = currentWeaponData.currentHeatInstance; // set currentHeat to weapons instance heat
            isOverheated = currentWeaponData.isOverHeatedInstance; // set overheat bool from isntance
        }
        else
        {
            currentHeat = 0; // set to 0 since no overheat for weapon
            isOverheated = false; // no overheat
        }
        // update overheat since we got new weapon now...

        Debug.Log("Now holding: " + newData.weaponData.weaponName);
    }

    public WeaponInstance GetWeaponInstance()
    {
        return currentWeaponData;
    }
}

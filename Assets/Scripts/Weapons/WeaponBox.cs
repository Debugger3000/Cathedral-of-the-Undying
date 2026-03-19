using UnityEngine;

public class WeaponBox : MonoBehaviour
{

    public WeaponName weaponName;

    public float boxLifeTime; // lifetime of box until it destroys itself ?

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    // private void OnCollisionEnter2D(Collision2D other) 
    // {
    //     Debug.Log("collider COLLISION was entered");        
    //     if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    // private void OnTriggerEnter2D(Collider2D other) 
    // {
    //     Debug.Log("collider trigger was entered");
    //     if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
    //     {
    //         Destroy(gameObject);
    //     }
    // }
    
    // call when we instantiate box object on enemy death...
    public void SetWeaponToBox(WeaponName weaponName)
    {
        this.weaponName = weaponName;
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

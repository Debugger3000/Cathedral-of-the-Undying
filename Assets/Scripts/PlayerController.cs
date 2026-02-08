

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb; // player rigidbody

    [Header("Master Weapon List")]
    public WeaponDatabase masterDB;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Aim")]
    public Transform aimPivot;
    // private Transform muzzlePoint;
    private Vector2 moveInput;
    private Vector2 mousePos; // mouse for aim pos
    

    [Header("Weapon")]
    // projectile prefab spawn
    public Weapon weaponControl; // Assign your gun object here
    // public WeaponData Pistol;
    //public GameObject curProjectile;
    //public GameObject curWeapon;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        WeaponData data = masterDB.GetWeaponByName("Shotgun");
        weaponControl.Equip(data);
    }

    
    

    // This MUST match the name of your Action in the Input Asset.
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        // Debug.Log(moveInput);
    }

    // Player aim with mouse
    public void OnAim(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
        Debug.Log(mousePos);
    }

    // Pplayer shoot with spacebar
    public void OnShoot(InputAction.CallbackContext context)
    {
        // fire only on space press down
        if (context.performed)
        {
            if (weaponControl != null)
            {
                // shootInput = context.ReadValue<>();
                Debug.Log("Shoot input / spacebar pressed !");

                // get muzzle pos
                // Vector3 muzzlePos = muzzlePoint.position;

                // initialize prefab - projectile script takes over...
                //Instantiate(curProjectile, muzzlePos, muzzlePoint.rotation);
                weaponControl.Fire(); // fire weapon via Weapon Class method
            }
            
        }
        

    }

    private void FixedUpdate()
    {
        // This makes for much smoother collisions
        float moveX = moveInput.x * moveSpeed;
        float moveY = moveInput.y * moveSpeed;
        rb.linearVelocity = new Vector2(moveX, moveY);

        // call aim player with fixed frames...
        AimPlayer();
    }

    private void Update()
    {
        
    }

    // mouse aim function
    private void AimPlayer()
    {
        //Debug.Log("aim player function called...");
        // Convert the mouse pixels to a point in your game world
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        // Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint();

        // Get the direction from the player to the mouse
        Vector2 lookDir = (Vector2)mouseWorldPos - (Vector2)aimPivot.position;


        // Calculate the angle in degrees
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        // Apply the rotation -- offset by -90 so player is facing up at start.
        rb.MoveRotation(angle - 90f);
        
        //transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}

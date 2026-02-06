

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public float moveSpeed = 5f;

    [Header("Aim")]
    [SerializeField] private Transform aimPivot;
    private Vector2 moveInput;
    private Vector2 mousePos; // mouse for aim pos
    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Use FixedUpdate for physics-based velocity
    void FixedUpdate()
    {
        // We set velocity directly to ensure it moves at a constant speed
        rb.linearVelocity = transform.up * speed; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("projectile detection.........");
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Environment")
        {
            Debug.Log("Hit an environment Layer!");
            HandleHit(gameObject);
        }
    }

    void HandleHit(GameObject target)
    {
        // Add damage logic here
        Destroy(target); // Destroy the bullet
    }
}

using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10;
    protected Rigidbody2D rb; // grab projectile gameObject rigidbody
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public void SetAttributes(float speed, float damage)
    {
        Debug.Log($"Set base projectile attributres too: {speed} {damage}");
        this.speed = speed;
        this.damage = damage;
    }

    public abstract void SpecialEffect();

    public virtual void EnemyHit(GameObject target)
    {


        Destroy(target); // destroy projectile...
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
        // base environment projectile destruction
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Environment")
        {
            Debug.Log("Hit an environment Layer!");
            HandleHit(gameObject);
        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Enemy")
        {
            Debug.Log("Hit an enemy with player projectile !!!");
            EnemyHit(gameObject);
            
        }
    }

    void HandleHit(GameObject target)
    {
        // Add damage logic here
        Destroy(target); // Destroy the bullet
    }

    
}

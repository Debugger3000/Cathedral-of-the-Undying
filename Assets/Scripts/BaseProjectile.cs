using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10;
    public int weaponPoints = 10;
    public bool isSpecialEffect = false;
    public WeaponDebuffData debuffData;
    protected Rigidbody2D rb; // grab projectile gameObject rigidbody
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public void SetAttributes(float speed, float damage, int weaponPoints, bool isSpecialEffect, WeaponDebuffData debuffData)
    {
        Debug.Log($"Set base projectile attributres too: {speed} {damage}");
        this.speed = speed;
        this.damage = damage;
        this.weaponPoints = weaponPoints; // set weaponPoints
        this.isSpecialEffect = isSpecialEffect; // special effect flag...
        this.debuffData = debuffData; // set weapon debuff data...
    }

    // Effects to apply to a unit hit by the projectile
    // can be a debuff or direct effect to unit (knockback, stun, etc....)
    public abstract DebuffController SpecialEffect(GameObject enemy, StatsCopy enemyStats, BaseProjectile projectile);

    // default projectile on hit logic - projectile gets destroyed
    // Implement in child projectile class if there is special on hit effect...
    public virtual void OnEnemyHit(GameObject target)
    {
        Destroy(target); // destroy projectile...
    }

    // Use FixedUpdate for physics-based velocity
    void FixedUpdate()
    {
        // We set velocity directly to ensure it moves at a constant speed
        rb.linearVelocity = transform.up * speed; 
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("projectile detection.........");
        // base environment projectile destruction
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Environment")
        {
            Debug.Log("Hit an environment Layer!");
            OnEnemyHit(gameObject);
        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Enemy")
        {
            Debug.Log("Hit an enemy with player projectile !!!");
            OnEnemyHit(gameObject);
            // point multiplier needs to be increased
            PointMultiplier.Instance.AddPoint(weaponPoints); // add weapon points...
        }
    }
}

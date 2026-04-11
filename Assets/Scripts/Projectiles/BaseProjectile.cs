using System;
using System.Collections.Generic;
using UnityEngine;


// Enemies can also use this class
public abstract class BaseProjectile : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10;
    public float armourPenetration;
    public int weaponPoints = 10;
    public bool isSpecialEffect = false;
    public WeaponDebuffData debuffData;
    protected Rigidbody2D rb; // grab projectile gameObject rigidbody
    protected Transform playerTarget;

    public bool hasDistanceEffect;
    public float distanceForDistanceEffect = 5f;
    public bool distanceEffectTriggered = false;

    public List<GameObject> childrenProjectiles = new List<GameObject>();

    private Vector2 projectileStartPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
       projectileStartPosition = rb.position;
    }

    // PLAYER PROJECTILE SETTER
    // Set From enemyData or weaponData
    public void SetAttributes(float speed, float damage, float armourPenetration, int weaponPoints, bool isSpecialEffect, WeaponDebuffData debuffData, bool hasDistanceEffect, float distanceForDistanceEffect, List<GameObject> childrenProjectiles)
    {
        //Debug.Log($"Set base projectile attributres too: {speed} {damage}");
        this.speed = speed;
        this.damage = damage;
        this.armourPenetration = armourPenetration;
        this.weaponPoints = weaponPoints; // set weaponPoints
        this.isSpecialEffect = isSpecialEffect; // special effect flag...
        this.debuffData = debuffData; // set weapon debuff data...
        this.hasDistanceEffect = hasDistanceEffect;
        this.distanceForDistanceEffect = distanceForDistanceEffect;
        this.childrenProjectiles = childrenProjectiles;
    }

    // ENEMY PROJECTILE SETTER
    public void SetEnemyAttributes(float speed, float damage, float armourPenetration, bool isSpecialEffect, WeaponDebuffData debuffData, Transform playerTarget)
    {
        //Debug.Log($"Set base projectile attributres too: {speed} {damage}");
        this.speed = speed;
        this.damage = damage;
        this.armourPenetration = armourPenetration;
        this.isSpecialEffect = isSpecialEffect; // special effect flag...
        this.debuffData = debuffData; // set weapon debuff data...
        this.playerTarget = playerTarget;
    }


    // Effects to apply to a unit hit by the projectile
    // can be a debuff or direct effect to unit (knockback, stun, etc....)
    public abstract Tuple<WeaponDebuffData, StatsCopy> SpecialEffect(GameObject enemy, StatsCopy enemyStats, BaseProjectile projectile);

    // default projectile on hit logic - projectile gets destroyed
    // Implement in child projectile class if there is special on hit effect...
    public virtual void OnEnemyHit(GameObject target)
    {
        Destroy(target); // destroy projectile...
    }

    public virtual void OnPlayerHit(GameObject target)
    {
        Destroy(target); // destroy projectile...
    }

    public virtual void OnEnvironmentHit(GameObject target)
    {
        Destroy(target); // destroy projectile...
    }

    protected virtual void ProjectileMovement()
    {
        // We set velocity directly to ensure it moves at a constant speed
        rb.linearVelocity = transform.up * speed; 
    }



    // Use FixedUpdate for physics-based velocity
    void FixedUpdate()
    {
        ProjectileMovement(); // projectile movement

        // on certain distance do something
        if (hasDistanceEffect && !distanceEffectTriggered)
        {
            float distance = Vector2.Distance(projectileStartPosition, rb.position);
            if (distance >= distanceForDistanceEffect)
            {
                distanceEffectTriggered = !distanceEffectTriggered;
                DistanceEffect(); // call distance effect...
            }
        }
    }


    protected virtual void DistanceEffect()
    {
        // do something

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Any and all projectiles that collide with environment
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Environment")
        {
            //Debug.Log("Hit an environment Layer!");
            OnEnvironmentHit(gameObject);
        }
        // Player Projectiles that hit enemy
        else if (LayerMask.LayerToName(collision.gameObject.layer) == "Enemy")
        {
            //Debug.Log("Hit an enemy with player projectile !!!");
            OnEnemyHit(gameObject);
            // point multiplier needs to be increased
            PointMultiplier.Instance.AddPoint(weaponPoints); // add weapon points...
        }
        // EnemyProjectiles that hit player
        else if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            //Debug.Log("Enemy projectile hit player");
            OnPlayerHit(gameObject);
        }
    }
}

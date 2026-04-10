using System;
using UnityEngine;


// hit box stats are within EnemyData
// B/C we can maintain enemy data in one place, especially its attack damage and such...
public abstract class AttackHitboxController : MonoBehaviour
{
    public float damage; // supplied by enemyData
    public float armourPenetration; //
    public float hitboxLifetime; // supplied by enemyData
    public int weaponPoints = 10;

    public WeaponDebuffData debuffData; // debuff data is there is a special effect on the attack

    // You could also add knockback or status effects here later!
    public bool isSpecialEffect; // special effects on hitbox..

    void Start()
    {
        // Debug.Log($"attackhit box damage: {damage}, lifetime: {hitboxLifetime}");
        // // Auto-destruct like we discussed earlier
        // Destroy(gameObject, hitboxLifetime); 
    }
    public void Setup(float dmg, float armourPenetration, int weaponPoints, float life, bool isHitBoxSpecialEffect, WeaponDebuffData debuffData)
    {
        damage = dmg;
        this.armourPenetration = armourPenetration;
        this.weaponPoints = weaponPoints;
        hitboxLifetime = life;
        isSpecialEffect = isHitBoxSpecialEffect;
        this.debuffData = debuffData;
        //Debug.Log($"Hitbox initialized with Damage: {damage} and Life: {hitboxLifetime}");
        //Destroy(gameObject, hitboxLifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Any and all projectiles that collide with environment
        
        // Player Projectiles that hit enemy
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Enemy")
        {
            // point multiplier needs to be increased
            PointMultiplier.Instance.AddPoint(weaponPoints); // add weapon points...
        }
        
    }

    public abstract Tuple<WeaponDebuffData, StatsCopy> HitBoxSpecialEffect(GameObject enemy, StatsCopy enemyStats); // child implements its special effect
}

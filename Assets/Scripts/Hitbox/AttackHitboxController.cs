using System;
using UnityEngine;


// hit box stats are within EnemyData
// B/C we can maintain enemy data in one place, especially its attack damage and such...
public abstract class AttackHitboxController : MonoBehaviour
{
    public float damage; // supplied by enemyData
    public float armourPenetration; //
    public float hitboxLifetime; // supplied by enemyData

    public WeaponDebuffData debuffData; // debuff data is there is a special effect on the attack

    // You could also add knockback or status effects here later!
    public bool isSpecialEffect; // special effects on hitbox..

    void Start()
    {
        // Debug.Log($"attackhit box damage: {damage}, lifetime: {hitboxLifetime}");
        // // Auto-destruct like we discussed earlier
        // Destroy(gameObject, hitboxLifetime); 
    }
    public void Setup(float dmg, float armourPenetration, float life, bool isHitBoxSpecialEffect, WeaponDebuffData debuffData)
    {
        damage = dmg;
        this.armourPenetration = armourPenetration;
        hitboxLifetime = life;
        isSpecialEffect = isHitBoxSpecialEffect;
        this.debuffData = debuffData;
        //Debug.Log($"Hitbox initialized with Damage: {damage} and Life: {hitboxLifetime}");
        //Destroy(gameObject, hitboxLifetime);
    }

    public abstract Tuple<WeaponDebuffData, StatsCopy> HitBoxSpecialEffect(GameObject enemy, StatsCopy enemyStats); // child implements its special effect
}

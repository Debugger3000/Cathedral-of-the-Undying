using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StatsCopy
{
    // These are the stats prone to modification via debuffs/buffs
    public float currentHealth;
    //  dots would just apply to actual enemy or player currentHealth but be controlled by debuff controller

    // public float maxHealth;
    // public float damage;
    public float moveSpeed;
    public float armour;

    public List<WeaponDebuffData> enemyEffectsToRemove = new List<WeaponDebuffData>();
    // public float attackCooldown;

    // Constructor to clone the ScriptableObject data into this instance
    public StatsCopy(float currentHealth, float moveSpeed, float armour)
    {
        // maxHealth = data.maxHealth;
        this.currentHealth = currentHealth;
        // damage = data.damage;
        this.moveSpeed = moveSpeed;
        this.armour = armour;
        // attackCooldown = data.attackCooldown;
    }
}

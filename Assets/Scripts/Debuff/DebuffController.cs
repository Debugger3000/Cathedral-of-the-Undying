using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DebuffController
{

    // Control debuff queue and lifetime for enemy and player debuffs
    // NO debuff stacking. we can just reset lifetime of debuff if reapplied such that id matches...

    public WeaponDebuffData debuffData;
    public StatsCopy debuffedStats;

    public StatsCopy stats;

    public float normalHealth; 
    public float normalmoveSpeed;
    public float normalArmour;

    public List<ActiveDebuff> activeDebuffs = new List<ActiveDebuff>(); // control a list of debuffs for a unit

    public bool isDot = false; // if debuff is a dot, logic applies differently...


    public DebuffController(WeaponDebuffData debuffData, StatsCopy debuffedStats)
    {
        this.debuffData = debuffData; // set debuff data
        this.debuffedStats = debuffedStats; // set debuffed stats copy
    }

    // to create a UNIT DEBUFF CONTROLLER INSTANCE which will manage that units debuffs...
    public DebuffController(StatsCopy stats)
    {
        this.stats = stats;
        normalmoveSpeed = stats.moveSpeed;
        normalArmour = stats.armour;
        normalHealth = stats.currentHealth;
    }

    public void SetDebuff(WeaponDebuffData debuffData, StatsCopy debuffedStats)
    {
        //this.debuffData = debuffData; // set debuff data
        //this.debuffedStats = debuffedStats; // set debuffed stats copy

        // add debuff
        AddDebuff(debuffData,debuffedStats);

        // affect UI
    }



    // debuff stuff
    // sourceType - 'player', 'enemy'
    public StatsCopy HandleDebuffTimers(string sourceType)
    {

        // Loop backwards so we can safely remove items while iterating
        for (int i = activeDebuffs.Count - 1; i >= 0; i--)
        {
            // check if debuff is a dot... if so, then we need to drop health 
            ActiveDebuff debuff = activeDebuffs[i];
        
            // before
            int secondsBefore = Mathf.FloorToInt(debuff.timeRemaining);

            debuff.timeRemaining -= Time.deltaTime;

            // after to check whole seconds...
            int secondsAfter = Mathf.FloorToInt(debuff.timeRemaining);

            // debuff should be removed
            if (debuff.timeRemaining <= 0)
            {
                
                RemoveDebuff(debuff.data, debuff.debuffedStats); // remove debuff from queue
                // UI
                // Player - Enemy 
                if (sourceType == "player")
                {
                    GameController.Instance.RemovePlayerDebuffUI(debuff.data); // remove debuff from player UI
                }
                else if(sourceType == "enemy")
                {
                    stats.enemyEffectsToRemove.Add(debuff.data); // add effect to return stats for enemycontroller to deal with
                }
                activeDebuffs.RemoveAt(i);
            }
            // debuff still good, check to reapply effects if better debuffs died
            else
            {
                CheckSimilarDebuffs(debuff.debuffedStats);
            }
            Debug.Log($"Debuffs in list: {activeDebuffs.Count}");
        }

        // return stats 
        return stats;
    }

    public float HandleDotTimers()
    {
        float dotDamageAggregate = 0f;
        // Loop backwards so we can safely remove items while iterating
        for (int i = activeDebuffs.Count - 1; i >= 0; i--)
        {
            // check if debuff is a dot... if so, then we need to drop health 
            ActiveDebuff debuff = activeDebuffs[i];
        
            // before
            int secondsBefore = Mathf.FloorToInt(debuff.timeRemaining);

            debuff.timeRemaining -= Time.deltaTime;

            // after to check whole seconds...
            int secondsAfter = Mathf.FloorToInt(debuff.timeRemaining);

            // dot logic, and when whole seconds are hit. Damage should happen
            if (secondsAfter < secondsBefore && debuff.timeRemaining > 0)
            {
                if (debuff.data.isDot) // Assuming your data has this bool
                {
                    dotDamageAggregate += debuff.data.damage; // flat dot damage
                }
            }

            // debuff should be removed
            if (debuff.timeRemaining <= 0)
            {
                if (debuff.data.isDot) // dots last tick should be when it expires
                {
                    dotDamageAggregate += debuff.data.damage; // dot damage is flat
                }
            }
            // debuff still good, check to reapply effects if better debuffs died
            else
            {
                CheckSimilarDebuffs(debuff.debuffedStats);
            }
            Debug.Log($"Debuffs in list: {activeDebuffs.Count}");
        }
        // return dot damage
        return dotDamageAggregate;
    }

    public void AddDebuff(WeaponDebuffData debuffData, StatsCopy debuffedStats)
    {
        // Optional: Check if debuff already exists to refresh timer instead of stacking
        ActiveDebuff existing = activeDebuffs.Find(d => d.data.debuffId == debuffData.debuffId);
        if (existing != null)
        {
            existing.timeRemaining = debuffData.lifetime;
            return;
        }

        // lets stack debuff hehe
        activeDebuffs.Add(new ActiveDebuff(debuffData, debuffedStats));
        // Apply initial stat changes here if needed
        //statsCopy = data.debuffedStats; // this sets everything to just this single debuff stats

        // compare debuffs for proper allocation
        CheckSimilarDebuffs(debuffedStats);


        // Debug.Log($"Debuff move speed is {data.debuffedStats.moveSpeed}...........");
        Debug.Log($"ORIGINAL move speed is {stats.moveSpeed}...........");

    }

    // apply more severe debuffs by overwriting less severe debuff
    private void CheckSimilarDebuffs(StatsCopy debuffedStats)
    {
        if (stats.moveSpeed != debuffedStats.moveSpeed)
        {
            // apply moveSpeed debuff to statsCopy
            // apply higher percent debuff
            if(stats.moveSpeed > debuffedStats.moveSpeed)
            {
                stats.moveSpeed = debuffedStats.moveSpeed; // apply worse debuff...
            }
        }

        // check armour too
        if (stats.armour != debuffedStats.armour)
        {
            // apply armour debuff to statsCopy
            if(stats.armour > debuffedStats.armour)
            {
                stats.armour = debuffedStats.armour; // apply worse debuff...
            }
        }
    }


    private void RemoveDebuff(WeaponDebuffData data,StatsCopy debuffedStats)
    {
        Debug.Log($"Debuff {data.debuffId} expired.");
        // Logic to revert statsCopy back to original values
        stats.moveSpeed = normalmoveSpeed;
        stats.armour = normalArmour;

        

        // apply lesser debuffs when larger ones are removed...
        //CheckSimilarDebuffs(debuffedStats);
    }
}

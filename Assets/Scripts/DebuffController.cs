using UnityEngine;

[System.Serializable]
public class DebuffController
{

    // Control debuff queue and lifetime for enemy and player debuffs
    // NO debuff stacking. we can just reset lifetime of debuff if reapplied such that id matches...

    public WeaponDebuffData debuffData;
    public StatsCopy debuffedStats;

    public bool isDot = false; // if debuff is a dot, logic applies differently...


    public DebuffController(WeaponDebuffData debuffData, StatsCopy debuffedStats)
    {
        this.debuffData = debuffData; // set debuff data
        this.debuffedStats = debuffedStats; // set debuffed stats copy
    }
}

using UnityEngine;

// Applies a flat deduction on statsCopy based on lifetime...
[System.Serializable]
public class ActiveDebuff
{
    public WeaponDebuffData data;
    public StatsCopy debuffedStats;
    public float timeRemaining;

    public ActiveDebuff(WeaponDebuffData data, StatsCopy debuffedStats)
    {
        this.data = data;
        timeRemaining = data.lifetime;
        this.debuffedStats = debuffedStats;
    }
}

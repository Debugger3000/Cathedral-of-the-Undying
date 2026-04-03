using UnityEngine;

public class Armour 
{
    // Armour
        // Armour should subtract flat from weapon damage
            // if armour makes weapon damage == 0, then we should buffer with a minimal percent of damage such as 5%... 
        // armour pen should subtract flat amount from armour
        // if Armour = 20
        // armour pen = 10 and damage is 15 
        // 20 - 10 = 10 armour --> 10 - 15 damage = gun does 5 damage 

    // Rules
    //  


    // Armour Stages
    // 0 armour --> player can use any weapon to kill with good damage

    // tankier melee
    //  10 armour --> shotgun should penetration 

    // control basic armour deduction math
    public float armourDeductionBase(float armourAmount, float armourPen, float damage)
    {
        // Armour Penetration Logic
        // 10 - 10 --> weapon does 100% of weapon damage...
        float armourLeftOver = armourAmount - armourPen;
        Debug.Log($"armour left over: {armourLeftOver}");
        if(armourLeftOver <= 0)
        {
            Debug.Log($"returning weapon damage: {damage}");
            return damage; // armour can only negate armour, but cannot add damage
        } 
        else
        {
            Debug.Log($"returning damage - armourleftover: {damage -armourLeftOver}");
            return damage - armourLeftOver; // 
        }
    }
}

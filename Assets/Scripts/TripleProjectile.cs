using UnityEngine;

public class TripleProjectile : BaseProjectile
{

    // this is shotgun projectile
    // shoot 3 bullets
    // knockback effect
    public override void SpecialEffect()
    {
        Debug.Log("The fireball exploded in a radius!");
        // Add knockback effect to enemies hit ???
    }
}

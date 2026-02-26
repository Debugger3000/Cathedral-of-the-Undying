using UnityEngine;

public class AttackHitboxController : MonoBehaviour
{
    public float damage; // dont set this on the script on the hitvox gameobject itself.. this data is given by enemyData
    public float hitboxLifetime; // dont set this on the script on the hitvox gameobject itself.. this data is given by enemyData
    // You could also add knockback or status effects here later!

    void Start()
    {
        // Debug.Log($"attackhit box damage: {damage}, lifetime: {hitboxLifetime}");
        // // Auto-destruct like we discussed earlier
        // Destroy(gameObject, hitboxLifetime); 
    }
    public void Setup(float dmg, float life)
    {
        damage = dmg;
        hitboxLifetime = life;
        Debug.Log($"Hitbox initialized with Damage: {damage} and Life: {hitboxLifetime}");
        Destroy(gameObject, hitboxLifetime);
    }
}

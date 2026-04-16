

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb; // player rigidbody

    // [Header("Master Weapon List")]
    // public WeaponDatabase masterDB;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Aim")]
    public Transform aimPivot;
    // private Transform muzzlePoint;
    private Vector2 moveInput;
    private Vector2 mousePos; // mouse for aim pos
    

    [Header("Weapon")]
    public BaseWeapon weaponControl; // Assign your gun object here
    private bool isFiring = false; // control held down space bar for shoot

    [Header("Health")]
    private float maxHealth = 100f;
    private float currentHealth;
    private float armour = 10f;
    private StatsCopy statsCopy; // copy of data to apply effects too...
    // public List<ActiveDebuff> activeDebuffs = new List<ActiveDebuff>();

    private DebuffController playerDebuffController;

    private bool isAffected = false; // for special effects 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // WeaponData data = masterDB.GetWeaponByName("Shotgun");
        // weaponControl.Equip(data);

        currentHealth = maxHealth; // set currentHealth
        statsCopy = new StatsCopy(maxHealth, moveSpeed, armour);
        playerDebuffController = new DebuffController(statsCopy);
    }

    // Player collision logic
    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Enemy has collided with the player...
        // Player should take damage based on enemy damage property
        // This only works for physical contact driven enemy interactions
        // if(LayerMask.LayerToName(collision.gameObject.layer) == "Enemy")
        // {
        //     //EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
        //     //PlayerTakeDamage(enemy.enemyData.damage); // give damage from enemy data
        // }
        // if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyAttack"))
        // {
        //     Debug.Log($"Enemy hitbox triggered player.................");
        //     // EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
        //     // If the player is currently dashing/dodging, we skip the damage
        //     // if (isInvulnerable) 
        //     // {
        //     //     Debug.Log("Dodged the attack!");
        //     //     return;
        //     // }

        //     PlayerTakeDamage(10f); // player takes damage...
        // }
        // else if 
        // enemy projectiles...
    }

    private void PlayPlayerHitAudio()
    {
        AudioManager.Instance.PlayAudioClip(AudioKey.PlayerHitByAttackGrunt); // player hit grunt
    }


    // To Detect certain trigger interactions
    // Enemy hitboxes
    // Weapon Boxes
    private void OnTriggerEnter2D(Collider2D other) 
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyAttack"))
        {
            Debug.Log("Enemy hitbox triggered player via TRIGGER event!");

            if (other.TryGetComponent<AttackHitboxController>(out var attackHitboxScript))
            {
                Debug.Log($"Hit by {other.gameObject.name} for {attackHitboxScript.damage} damage!");

                // if hitbox has damage to it
                if (attackHitboxScript.damage > 0)
                {
                    PlayerTakeDamage(attackHitboxScript.damage, attackHitboxScript.armourPenetration); // player takes damage
                }

                // apply whatever special effects of hitbox attack to player
                // check if enemy has hitbox Special effects
                if (attackHitboxScript.isSpecialEffect)
                {
                    StartCoroutine(HitBoxSpecialEffect(attackHitboxScript)); // apply special effects
                }

                // player grunts on hit
                PlayPlayerHitAudio();
            }

        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
            Debug.Log("Enemy projectile hit player............");

            if (other.TryGetComponent<BaseProjectile>(out var baseProjectileScript))
            {
                Debug.Log($"Hit by {other.gameObject.name} for {baseProjectileScript.damage} damage!");

                PlayerTakeDamage(baseProjectileScript.damage, baseProjectileScript.armourPenetration); // player takes damage from projectile

                // apply whatever special effects of hitbox attack to player
                // check if enemy has hitbox Special effects
                // if (attackHitboxScript.isSpecialEffect)
                // {
                //     StartCoroutine(HitBoxSpecialEffect(attackHitboxScript)); // apply special effects
                // }

                if (baseProjectileScript.isSpecialEffect)
                {
                    StartCoroutine(ProjectileSpecialEffect(baseProjectileScript));
                }

                // player grunts on hit
                PlayPlayerHitAudio();

            }

        }
        // player runs into a weapon box...
        else if(other.gameObject.layer == LayerMask.NameToLayer("WeaponBox"))
        {
            // Use GetComponentInParent to be safe
            WeaponBox boxScript = other.GetComponentInParent<WeaponBox>();

            if (boxScript != null) 
            {
                WeaponName name = boxScript.weaponName;
                Debug.Log($"Weapon name on player pickup is: {name}");
                
                GameController.Instance.PickUpWeaponBox(name);
                
                // Tell the box to destroy itself so it's not picked up twice
                boxScript.SelfDestruct();
            }
            else 
            {
                Debug.LogWarning("Hit a 'WeaponBox' layer, but no WeaponBox script was found!");
            }
        }
    }

    // Debuff / Special effects on attacks
    private IEnumerator HitBoxSpecialEffect(AttackHitboxController attackHitboxScript)
    {
        isAffected = true; 
        
        // Apply the knockback/effect and set statsCopy
        // projectile special effects can either debuff a unit or apply whatever logic at end of projectile
        Tuple<WeaponDebuffData, StatsCopy> debuffTuple = attackHitboxScript.HitBoxSpecialEffect(gameObject, statsCopy);

        WeaponDebuffData data = debuffTuple.Item1;
        StatsCopy stats = debuffTuple.Item2;
        playerDebuffController.SetDebuff(data,stats);

        // Affect Player UI with debuff
        GameController.Instance.AddPlayerDebuffUI(data);

        // Wait for a short duration so the physics force actually moves the object
        yield return new WaitForSeconds(0.2f); 
        
        isAffected = false;
    }

    private IEnumerator ProjectileSpecialEffect(BaseProjectile projectile)
    {
        isAffected = true; 
        // Apply the knockback/effect and set statsCopy
        // projectile special effects can either debuff a unit or apply whatever logic at end of projectile
        Tuple<WeaponDebuffData, StatsCopy> debuffTuple = projectile.SpecialEffect(gameObject,statsCopy,projectile);

        WeaponDebuffData data = debuffTuple.Item1;
        StatsCopy stats = debuffTuple.Item2;
        playerDebuffController.SetDebuff(data,stats);

        // Affect Player UI with debuff
        GameController.Instance.AddPlayerDebuffUI(data);

        // Wait for a short duration so the physics force actually moves the object
        yield return new WaitForSeconds(0.2f); 
        
        isAffected = false;
    }


    // -------------------------------------------------------------------------
    // Player takes damage with armour reduction applied...
    public void PlayerTakeDamage(float damage, float armourPenetration)
    {
        // run through armour calculations first...
        float damageTaken = GameController.Instance.armourClass.armourDeductionBase(armour, armourPenetration, damage);
        Debug.Log($"Player took damage: {damageTaken}");
        currentHealth -= damageTaken; // subtract
        GameController.Instance.PlayerDamaged(currentHealth); // update UI for player health bar + check death condition

    }
    // flat damage for dots and other sources
    public void FlatPlayerTakeDamage(float damageTaken)
    {
        Debug.Log($"Flat damage to player is: {damageTaken} to health of: {currentHealth}");
        currentHealth -= damageTaken; // subtract
        GameController.Instance.PlayerDamaged(currentHealth); // update UI for player health bar + check death condition
        Debug.Log($"flat dmg taken health is now: {currentHealth}");

    }


    //------------------------------------
    // Player Inputs
    //
    // 'Q' - user input to change weapon
    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // rotate weapon in inventory...
            Debug.Log("Pressed Q to get inventory...");
            GameController.Instance.SwitchWeapon(); // call gameController to swap weapon...
            Debug.Log("After sweapon has been swapped.....");
            // if (weaponControl.GetWeaponInstance().weaponData.name == "Flamer")
            // {
            //     // slow movement down
            //     statsCopy.moveSpeed = statsCopy.moveSpeed * weaponControl.GetWeaponInstance().weaponData.
            // }
        }
    }
    
    // This MUST match the name of your Action in the Input Asset.
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        // Debug.Log(moveInput);
    }

    // Player aim with mouse
    public void OnAim(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
        //Debug.Log(mousePos);
    }

    // Pplayer shoot with spacebar
    public void OnShoot(InputAction.CallbackContext context)
    {
        // fire only on space press down
        // if (context.performed)
        // {
        //     if (weaponControl != null)
        //     {
        //         weaponControl.PlayerAttackWithWeapon(transform);
        //     }
        // }
        if (context.started) 
        {
            isFiring = true;
        }
        else if (context.canceled) 
        {
            isFiring = false;
        }

        
    }

    private void FixedUpdate()
    {
        // This makes for much smoother collisions
        float moveX = moveInput.x * statsCopy.moveSpeed;
        float moveY = moveInput.y * statsCopy.moveSpeed;
        rb.linearVelocity = new Vector2(moveX, moveY);

        // call aim player with fixed frames...
        AimPlayer();

        // only do debuff logic when player has debuffs
        if(playerDebuffController.activeDebuffs.Count > 0)
        {
            var (updatedStats, dotDmg) = playerDebuffController.HandleDebuffTimers("player");
            statsCopy = updatedStats;
            if (dotDmg != 0){
                FlatPlayerTakeDamage(dotDmg);
            }

            // check debuff statuses
            // statsCopy = playerDebuffController.HandleDebuffTimers("player"); // when we remove DEBUFFS
            // float dotDamageAggregate = playerDebuffController.HandleDotTimers(); // dot damage aggregate
            // if(dotDamageAggregate > 0)
            // {
            //     FlatPlayerTakeDamage(dotDamageAggregate); // apply dot damage
            // }
        }
        
        
    }

    private void Update()
    {
        if (weaponControl != null && isFiring)
        {
            weaponControl.PlayerAttackWithWeapon(transform);
        }
        // weapon stops firing... do something
        else if(weaponControl != null && !isFiring)
        {
            weaponControl.PlayerStopsAttackWithWeapon(transform);
        }
        
    }

    public void HealPlayer()
    {
        float healthAfter =  currentHealth + 20;
        if (healthAfter >= 100)
        {
            currentHealth = 100;
        }
    }

    // mouse aim function
    private void AimPlayer()
    {
        //Debug.Log("aim player function called...");
        // Convert the mouse pixels to a point in your game world
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        // Vector2 MouseWorldPos = Camera.main.ScreenToWorldPoint();

        // Get the direction from the player to the mouse
        Vector2 lookDir = (Vector2)mouseWorldPos - (Vector2)aimPivot.position;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        // Apply the rotation -- offset by -90 so player is facing up at start.
        rb.MoveRotation(angle - 90f);
        
        //transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}

using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Base enemy implementation
public abstract class EnemyController : MonoBehaviour
{
    // Base Settings
    private Transform target;
    private Transform myTransform;
    public Transform healthBarTransform; // can set on enemy prefab...
    private Rigidbody2D rb;
    public EnemyData enemyData; // Drag your ShamblerData or BossData here

    // Unit Stats
    protected float currentHealth;

    // Enemy UI
    public GameObject healthBarPrefab;
    public Image healthBarFill;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        myTransform = transform; // assign own transform
        rb = GetComponent<Rigidbody2D>(); // get rigidbody
        target = GameObject.FindGameObjectWithTag("Player").transform; // get player transform to follow...
        currentHealth = enemyData.maxHealth; // set health...
        // healthBarPrefab.SetActive = false;

        // Canvas canvas = GetComponentInChildren<Canvas>();
        // canvas.worldCamera = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enemy unit hit by something");
        // base environment projectile destruction
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Projectile")
        {
            Debug.Log("Hit an environment Layer!");

            BaseProjectile projectile = collision.GetComponent<BaseProjectile>(); // grab projectiles damage

            TakeDamage(projectile.damage); // unit should lose health...
        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            Debug.Log("Enemy !!!");
            //EnemyHit(gameObject);
            
        }
    }

    // receive flat damage...
    public void TakeDamage(float amount)
    {
        currentHealth -= amount; // subtract health with normal (0-100)
        Debug.Log($"Damage taken is: {amount}");

        healthBarFill.fillAmount = currentHealth / 100; //

        if (currentHealth <= 0) Die();
    }

    // implemented by actual unit script...
    protected abstract void Die(); // unit death



    // Update 
    void Update()
    {
        //rotate to look at player
        Vector3 direction = target.position - myTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        myTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));

        //move towards the player
        Vector3 newPosition = Vector3.MoveTowards(rb.position, target.position, enemyData.moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }
}

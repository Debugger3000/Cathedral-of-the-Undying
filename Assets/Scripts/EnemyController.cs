using Mono.Cecil.Cil;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 2f;
    public float rotationSpeed = 3f;

    public Transform myTransform;
    public Rigidbody2D rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        myTransform = transform;
        rb = GetComponent<Rigidbody2D>();
    } 
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //rotate to look at player
        Vector3 direction = target.position - myTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        myTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));

        //move towards the player
        Vector3 newPosition = Vector3.MoveTowards(rb.position, target.position, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }
}

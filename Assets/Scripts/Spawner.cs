using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Transform spawnLocation; // spawn units here

    // -------------------------------
    public EnemyData shamblerData; // currently hardcodedd
    // -------------------------------
    public float spawnInterval = 3f; // Set your delay here
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime; // Adds the time passed since last frame

        if (timer >= spawnInterval)
        {
            GameObject unit = Instantiate(shamblerData.enemyPrefab, transform.position, transform.rotation);
            timer = 0f; // Reset the timer
            unit.GetComponent<EnemyController>().enemyData = shamblerData;
        }
    }
}

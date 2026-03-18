using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Transform spawnLocation; // spawn units here

    // -------------------------------
    public List<EnemyData> enemyList; // list of enemy units

    // -------------------------------
    public float spawnInterval = 3f; // Set your delay here
    private float timer;
    private int unitIndex = 0; // index to alternate between two units for now

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
            EnemyData enemyData = enemyList[unitIndex];
            GameObject unit = Instantiate(enemyData.enemyPrefab, spawnLocation.position, spawnLocation.rotation);

            // simple switch to alternate between units
            if(unitIndex == 0)
            {
                unitIndex = 1;
            }
            else
            {
                unitIndex = 0;
            }

            timer = 0f; // Reset the timer
            unit.GetComponent<EnemyController>().enemyData = enemyData; // set enemy unit data onto units live script
        }
    }
}

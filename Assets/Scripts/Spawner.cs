using System.Collections.Generic;
using UnityEngine;


public enum EnemyName {
    Shambler,
    Shooter,
    Tracker,
    Sludger,
    Shielder,
    Demon
}

// Class data structure for ALL weapons and their drop level in relation to points multiplayer
[System.Serializable]
public class EnemyEntry {
    public EnemyName enemyName; // Choose enemy
    public EnemyData enemyData; // data for enemy
    public int weight;     // associated weight for procedural spawning system
}

public enum SpawnSettings
{
    FlipFlop, // back n forth
    Randomize, // random...
}

[System.Serializable]
public class SpawnerEntry {
    public List<EnemyName> enemyToSpawn; // Choose enemy
    public int curEnemyIndex = 0;
    public int timesToSpawnPattern; // how many times are we spawning this pattern
    public int intervalBetweenSpawns; // how many seconds between spawns
    public SpawnSettings spawnSettings; // where / how to spawn enemies (top or bot...)
}


public class Spawner : MonoBehaviour
{

    public Transform topSpawnLocation; // top spawner location
    public Transform botSpawnLocation; //bottom spawner location

    // -------------------------------
    public List<EnemyEntry> enemyList; // list of enemy units

    // Queue for pre-determined rounds...
        // Queue pattern based
            // Determine pattern of enemy releases
        // give how many times we spawn a single pattern
        // some sort of determinent for spawn location
            // do either flip flop settings, or randomize, or stick to one...

    public List<SpawnerEntry> spawnRounds;
    private int curRoundIndex = 0; // controls which round we are currently on...
        // when over last spawnRounds, we move to procedural generation...

    // 0 - Top spawner
    // 1 - Bot spawner
    private int spawnLocationSwitch = 0; // determine spawn location


    // -------------------------------
    private float timer;
    public float curSpawnInterval = 3f; // Set your delay here
        // DEFAULT: 3 seconds...
    

    //private int unitIndex = 0; // index to alternate between two units for now

    // PROCEDURAL SPAWNER   
    private bool proceduralSpawnSwitch = false;
    private SpawnerEntry proceduralEnemyList = new SpawnerEntry();
    public int proceduralEnemyListSize = 10;
    //private int demonCap = 3; // cap at 3 demons

    private Dictionary<int, EnemyName> enemyNameMap = new Dictionary<int, EnemyName>
    {
        { 0, EnemyName.Shambler },
        { 1, EnemyName.Shooter },
        { 2, EnemyName.Tracker },
        { 3, EnemyName.Sludger },
        { 4, EnemyName.Shielder },
        { 5, EnemyName.Demon }
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // Adds the time passed since last frame

        // might need switch for passing this block when we move to PROCEDURAL.
        // -------------
        if (!proceduralSpawnSwitch)
        {   
            // check to see if rounds are over
            // Check our pre determined rounds, and if index is within range of rounds list
            if (curRoundIndex < spawnRounds.Count)
            {
                if (timer >= spawnRounds[curRoundIndex].intervalBetweenSpawns)
                {
                
                    // spawn enemies for our round
                    SpawnerEntry curRound = spawnRounds[curRoundIndex];
                    // make sure timesToSpawn is greater than 0...
                    // we still have x amount of times to spawn our pattern
                    if (curRound.timesToSpawnPattern > 0)
                        {
                            
                        // spawn our current index...
                        int curSpawnIndex = curRound.curEnemyIndex; // get spawn index
                        Transform spawnLocation = DetermineSpawnType(curRound.spawnSettings); // get spawn location
                        EnemyData curEnemyData = GetEnemyDataFromName(curRound.enemyToSpawn[curSpawnIndex]); // get enemy data
                        GameObject unit = Instantiate(curEnemyData.enemyPrefab, spawnLocation.position, spawnLocation.rotation); // set prefab instantiate
                        unit.GetComponent<EnemyController>().enemyData = curEnemyData; // set enemy unit data onto units live script

                        // spawn pattern index check
                        if (curSpawnIndex == curRound.enemyToSpawn.Count-1)
                        {
                            // spawn index is on last index of spawn pattern... 
                            curRound.curEnemyIndex = 0; // reset to 0
                            curRound.timesToSpawnPattern -= 1; // decrement timesToSpawn
                        }
                        else
                        {
                            curRound.curEnemyIndex += 1; // increment curEnemyIndex
                        }
                        timer = 0f; // Reset the timer
                    }
                    // times to spawn is now 0, so we need to go to next round...
                    else
                    {
                        curRoundIndex += 1; // Go to next round
                        Debug.Log($"CURROUNDINDEX: {curRoundIndex}");
                    }
                }
            }
            // outside of rounds, go to procedural
                else
                {
                    proceduralSpawnSwitch = true; // flip to procedural spawner...
                    Debug.Log($"switch for procedural switch: {proceduralSpawnSwitch}");
                    GenerateProceduralEnemyList();
                    timer = 0f; // Reset the timer
                    
                }
        }
        // PROCEDURAL SPAWNER
        else
        {
            Debug.Log($"within procedural block...");
            // make sure procedural list is populated
            if (proceduralEnemyList.enemyToSpawn.Count == 10)
            {
                Debug.Log($"within 2nd procedural block... : {proceduralEnemyList.enemyToSpawn.Count}");
                // check timer
                if (timer >= proceduralEnemyList.intervalBetweenSpawns)
                {
                    Debug.Log($"within 3rd procedural block... : {proceduralEnemyList.intervalBetweenSpawns}");
                    //
                    // 
                    if(proceduralEnemyList.timesToSpawnPattern > 0)
                    {
                        Debug.Log($"within 4th procedural block... : SPAWNING AN ENEMY....");
                        // spawn our current index...
                        int curSpawnIndex = proceduralEnemyList.curEnemyIndex; // get spawn index
                        Transform spawnLocation = DetermineSpawnType(proceduralEnemyList.spawnSettings); // get spawn location
                        EnemyData curEnemyData = GetEnemyDataFromName(proceduralEnemyList.enemyToSpawn[curSpawnIndex]); // get enemy data
                        GameObject unit = Instantiate(curEnemyData.enemyPrefab, spawnLocation.position, spawnLocation.rotation); // set prefab instantiate
                        unit.GetComponent<EnemyController>().enemyData = curEnemyData; // set enemy unit data onto units live script

                        // spawn pattern index check
                        if (curSpawnIndex == proceduralEnemyList.enemyToSpawn.Count-1)
                        {
                            // spawn index is on last index of spawn pattern... 
                            proceduralEnemyList.curEnemyIndex = 0; // reset to 0
                            proceduralEnemyList.timesToSpawnPattern -= 1; // decrement timesToSpawn
                        }
                        else
                        {
                            proceduralEnemyList.curEnemyIndex += 1; // increment curEnemyIndex
                        }
                        timer = 0f; // Reset the timer
                    }
                    // times to spawn has reached 0, so we make new list...
                    else
                    {
                        Debug.Log($"gENERATING NEW LIST FROM WITHIN PROCREDURAL BLOCKER");
                        GenerateProceduralEnemyList(); // generate new list
                        timer = 0f; // Reset the timer
                    }
                }
            }
        }
    }

    // returns spawn location via SpawnSetting
    private Transform DetermineSpawnType(SpawnSettings spawnSetting)
    {
        int index = 0;
        if(spawnSetting == SpawnSettings.Randomize)
        {
            index = Random.Range(0, 2);
        }
        else if(spawnSetting == SpawnSettings.FlipFlop)
        {
            if(spawnLocationSwitch == 0)
            {
                index = 0;
                spawnLocationSwitch = 1;
            }
            else
            {
                index = 1;
                spawnLocationSwitch = 0;
            }
        }
        // get transform
        if (index == 0)
        {
            // return top Transform spawn location
            return topSpawnLocation;
        }
        else
        {
            // return bot Transform spawn location
            return botSpawnLocation;
        }
        
    }

    // Get unit data for EnemyName
    private EnemyData GetEnemyDataFromName(EnemyName enemyName)
    {
        foreach (EnemyEntry enemyEntry in enemyList)
        {
            if (enemyEntry.enemyName == enemyName)
            {
                return enemyEntry.enemyData;
            }
        }
        // should never return null but yeah
        return null;
    }

    private int GenerateRandomNumber(int baseSize, int size)
    {
        return Random.Range(baseSize, size);
    }


    // PROCEDURAL SPAWNER

    private void GenerateProceduralEnemyList()
    {
        Debug.Log("GENERATING PROCEDURAL LSIT....");
        // spawn at least one demon per list
        int indexToSpawnDemon = GenerateRandomNumber(0,11);
        
        proceduralEnemyList.enemyToSpawn = new List<EnemyName>(); // initialize list
        // randomize time times from 0 - 5
        for(int i = 0; i < proceduralEnemyListSize; i++)
        {
            // add demon
            if (i == indexToSpawnDemon)
            {
                proceduralEnemyList.enemyToSpawn.Add(EnemyName.Demon); // add demon
            }
            else
            {
                int index = GenerateRandomNumber(0,enemyList.Count);
                EnemyName enemyName = enemyNameMap[index]; // grab enemyName

                proceduralEnemyList.enemyToSpawn.Add(enemyName); // enemy name to list
                
            }
            
        }
        // one time settings per spawnEntry
        proceduralEnemyList.timesToSpawnPattern = GenerateRandomNumber(2,4); // spawn pattern 2 - 4 times
        proceduralEnemyList.intervalBetweenSpawns = GenerateRandomNumber(2,4); // interval always 3..
        int spawnLocationType = GenerateRandomNumber(0,2);
        if (spawnLocationType == 0)
        {
            proceduralEnemyList.spawnSettings = SpawnSettings.FlipFlop;
        }
        else
        {
            proceduralEnemyList.spawnSettings = SpawnSettings.Randomize;
        }
        Debug.Log($"PROCEDURAL LSIT: {proceduralEnemyList}");
    }
}

using UnityEngine;

public class PointMultiplier : MonoBehaviour
{
    public static PointMultiplier Instance { get; private set; } // expose instance

    // public int currentScore = 0;
    public int multiplierPoints = 0; // 0 - 100 points within each level (1x,2x,3x,4x, etc...)
    public int multiplierPointsCeiling = 100; // how high our points can go
    public int multiplierLevel = 0; // default is 0. increment when points hit 100, decrement when time runs out... 
    public int multiplierLevelCeiling = 10; // // how high our multiplier can go...
    public float multiplierTimer = 0f;
    public float resetTime = 3.0f; // Multiplier resets after 3 seconds of no hits

    void Awake()
    {
        // Singleton Logic: Ensure only one exists
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    void Update()
    {
        // Handle multiplier decay over time
        if (multiplierTimer > 0 && !(multiplierLevel == 0 && multiplierPoints == 0))
        {
            multiplierTimer -= Time.deltaTime; // subtract time from timer
            UpdateTimerUI(); // update ui as this progress from 3 -> 0
        }
        else
        {
            // timer ran out...
            // subtract a level
            if(multiplierLevel > 0)
            {
                multiplierLevel -= 1; // subtract a level
                ResetTimer(); // reset timer..
                UpdateLevelUI(); // update level ui
            }
            
            multiplierPoints = 0; // reset points to zero
            UpdatePointsUI();
            
           
        }
    }

    // means a projectile hit...
    public void AddPoint(int baseAmountPoints)
    {

        // Increase multiplier points
        multiplierPoints += baseAmountPoints;


        if(multiplierPoints == 100 && multiplierLevel < multiplierLevelCeiling)
        {
            multiplierLevel++; // increment level
            multiplierPoints = 0; // reset multiplier points to 0
            UpdateLevelUI(); // update the level UI

            // check what weapons / upgrades are now available to drop on kills..
        }


        ResetTimer(); // reset timer var
        UpdateTimerUI(); // point achieved so we reset timer UI
        UpdatePointsUI(); // update points since we got a new one...

        

        // Debug.Log($"Score: {currentScore} (x{multiplier}!)");
    }

    private void UpdatePointsUI()
    {
        GameController.Instance.uiManager.UpdatePointsMultiplierPoints(multiplierPoints); // give it all update information
    }

    private void UpdateLevelUI()
    {
        GameController.Instance.uiManager.UpdateMultiplierLevel(multiplierLevel);
        
    }

    private void UpdateTimerUI()
    {
        GameController.Instance.uiManager.UpdateMultiplierTimer(multiplierTimer);
        
    }

    private void ResetTimer()
    {
        multiplierTimer = resetTime; // Refresh the decay timer
    }
}

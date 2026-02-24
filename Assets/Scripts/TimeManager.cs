using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public UIManager uiManager; // Drag your Canvas/UIManager here

    void Update() 
    {
        // Just send the raw float to the UI
        uiManager.UpdateTimerDisplay(Time.time);
    }
}

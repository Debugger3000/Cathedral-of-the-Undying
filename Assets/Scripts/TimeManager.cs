using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public UIManager uiManager; // Drag your Canvas/UIManager here

    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        float elapsed = Time.time - startTime;
        uiManager.UpdateTimerDisplay(elapsed);
    }
}

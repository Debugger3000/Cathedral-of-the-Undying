using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public UIManager uiManager; // Drag your Canvas/UIManager here

    private float startTime;
    public float elapsed;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        elapsed = Time.time - startTime;
        uiManager.UpdateTimerDisplay(elapsed);
    }
}

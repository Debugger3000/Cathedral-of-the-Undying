using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("Timer / Game")]
    [SerializeField] public TextMeshProUGUI timerText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Player")]
    public Image playerHealthBar;

    public Image playerCurrentWeapon;

    [Header("Points Multiplier")]
    public Image multiplierPointsProgressBar;
    public Image multiplierTimerProgressBar;
    public TextMeshProUGUI multiplierLevelText;
    public char multiplierlevelCharacter = 'X'; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCurrentWeaponDisplay(Sprite weaponSprite)
    {
        playerCurrentWeapon.sprite = weaponSprite; // set sprite ???
    }

    // set multiplier points progress bar 
    // 0 -> 100 (scaled fill range: 0 -> 1)
    public void UpdatePointsMultiplierPoints(int multiplierPoints)
    {
        // set progress bar for timer
        // 3 -> 0 how to scale this to 0 -> 1.0
        // Debug.Log($"Set POINTS FILL from: {multiplierPoints}");
        float scaledRange = multiplierPoints / 100.0f;
        // Debug.Log($"Set SCALED SCALED TO (for else): {scaledRange}");
        if(scaledRange == 1)
        {
            // we need to reset points bar to zero since we hit ceiling
            multiplierPointsProgressBar.fillAmount = 0; // set to 0...
            
        }
        else
        {
            
            multiplierPointsProgressBar.fillAmount = scaledRange; // set progress bar            
        }
        


        // set level
    }

    public void UpdateMultiplierLevel(int multiplierLevel)
    {
        string newLevelText = $"{multiplierLevel}{multiplierlevelCharacter}";
        multiplierLevelText.text = newLevelText; // set UI text
        
    }

    // set multiplier timer progress bar
    // 3 -> 0 timer converted to 0 -> 1.0 
    public void UpdateMultiplierTimer(float multiplierTimer)
    {
        float scaledRange = multiplierTimer / 3; // 
        multiplierTimerProgressBar.fillAmount = scaledRange;
        
    }

    // Update player health bar
    // receive whole number health (0 - 100)
    public void UpdatePlayerHealth(float currentHealth)
    {
        float adjustedHealth = currentHealth / 100; // get value (0 - 1)
        playerHealthBar.fillAmount = adjustedHealth; // set fill amount
    }

    // Update game timer display 
    public void UpdateTimerDisplay(float timeInSeconds) 
    {
        int minutes = (int)timeInSeconds / 60;
        int seconds = (int)timeInSeconds % 60;
        
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

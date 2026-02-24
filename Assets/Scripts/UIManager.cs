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

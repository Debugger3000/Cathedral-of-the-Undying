using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameMenuManager : MonoBehaviour
{
    [Header("Menu Button")]
    [SerializeField] private Button menuButton;

    [Header("Overlay Pages")]
    [SerializeField] private GameObject menuOverlay;
    [SerializeField] private GameObject menuPage;
    [SerializeField] private GameObject howToPlayPage;
    [SerializeField] private GameObject deathPage;

    [Header("Menu Buttons")]
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitGameButton;
    [SerializeField] private Button resumeButton;

    [Header("How To Play Buttons")]
    [SerializeField] private Button howToPlayBackButton;

    [Header("On Death Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button goToMainMenuButton;
    [SerializeField] private Button exitGameButtonOnDeath;
    public TextMeshProUGUI timeSurvivedText;

    private void OnEnable()
    {
        menuButton.onClick.AddListener(OnMenuClicked);
        howToPlayButton.onClick.AddListener(OnHowToPlayClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        exitGameButton.onClick.AddListener(OnExitGameClicked);
        resumeButton.onClick.AddListener(OnResumeClicked);
        howToPlayBackButton.onClick.AddListener(OnHowToPlayBackClicked);
        newGameButton.onClick.AddListener(OnNewGameClicked);
        goToMainMenuButton.onClick.AddListener(OnMainMenuClicked);
        exitGameButtonOnDeath.onClick.AddListener(OnExitGameClicked);
    }

    private void OnDisable()
    {
        menuButton.onClick.RemoveListener(OnMenuClicked);
        howToPlayButton.onClick.RemoveListener(OnHowToPlayClicked);
        mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
        exitGameButton.onClick.RemoveListener(OnExitGameClicked);
        resumeButton.onClick.RemoveListener(OnResumeClicked);
        howToPlayBackButton.onClick.RemoveListener(OnHowToPlayBackClicked);
        newGameButton.onClick.RemoveListener(OnNewGameClicked);
        goToMainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
        exitGameButtonOnDeath.onClick.RemoveListener(OnExitGameClicked);
    }

    private void Start()
    {
        CloseMenu();
    }

    private void OnMenuClicked()
    {
        Debug.Log("[GameMenu] Menu opened");
        menuOverlay.SetActive(true);
        menuPage.SetActive(true);
        howToPlayPage.SetActive(false);
        Time.timeScale = 0f;
    }

    private void OnResumeClicked()
    {
        Debug.Log("[GameMenu] Resume clicked");
        CloseMenu();
    }

    private void OnHowToPlayClicked()
    {
        Debug.Log("[GameMenu] How To Play clicked");
        menuPage.SetActive(false);
        howToPlayPage.SetActive(true);
    }

    private void OnHowToPlayBackClicked()
    {
        Debug.Log("[GameMenu] Back from How To Play");
        howToPlayPage.SetActive(false);
        menuPage.SetActive(true);
    }

    private void OnMainMenuClicked()
    {
        Debug.Log("[GameMenu] Main Menu clicked");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnExitGameClicked()
    {
        Debug.Log("[GameMenu] Exit Game clicked");
        Application.Quit();
    }

    private void CloseMenu()
    {
        menuOverlay.SetActive(false);
        howToPlayPage.SetActive(false);
        Time.timeScale = 1f;
    }

    public void DeathMenu(float timeSurvived)
    {
        int minutes = Mathf.FloorToInt(timeSurvived / 60f);
        int seconds = Mathf.FloorToInt(timeSurvived % 60f);
        timeSurvivedText.text = $"Time Survived: {minutes:00}:{seconds:00}";
        menuOverlay.SetActive(true);
        menuPage.SetActive(false);
        howToPlayPage.SetActive(false);
        deathPage.SetActive(true); //
    }

    private void OnNewGameClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
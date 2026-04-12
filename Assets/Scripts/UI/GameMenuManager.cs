using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    [Header("Menu Button")]
    [SerializeField] private Button menuButton;

    [Header("Overlay Pages")]
    [SerializeField] private GameObject menuOverlay;
    [SerializeField] private GameObject menuPage;
    [SerializeField] private GameObject howToPlayPage;

    [Header("Menu Buttons")]
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitGameButton;
    [SerializeField] private Button resumeButton;

    [Header("How To Play Buttons")]
    [SerializeField] private Button howToPlayBackButton;

    private void OnEnable()
    {
        menuButton.onClick.AddListener(OnMenuClicked);
        howToPlayButton.onClick.AddListener(OnHowToPlayClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        exitGameButton.onClick.AddListener(OnExitGameClicked);
        resumeButton.onClick.AddListener(OnResumeClicked);
        howToPlayBackButton.onClick.AddListener(OnHowToPlayBackClicked);
    }

    private void OnDisable()
    {
        menuButton.onClick.RemoveListener(OnMenuClicked);
        howToPlayButton.onClick.RemoveListener(OnHowToPlayClicked);
        mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
        exitGameButton.onClick.RemoveListener(OnExitGameClicked);
        resumeButton.onClick.RemoveListener(OnResumeClicked);
        howToPlayBackButton.onClick.RemoveListener(OnHowToPlayBackClicked);
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
}
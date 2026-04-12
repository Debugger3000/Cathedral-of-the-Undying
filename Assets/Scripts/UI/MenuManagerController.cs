using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManagerController : MonoBehaviour
{
    [Header("Pages")]
    [SerializeField] private GameObject mainMenuPage;
    [SerializeField] private GameObject controlsPage;

    [Header("Main Menu Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button exitButton;

    [Header("Controls Page Buttons")]
    [SerializeField] private Button backButton;

    private void OnEnable()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        controlsButton.onClick.AddListener(OnControlsClicked);
        exitButton.onClick.AddListener(OnExitClicked);
        backButton.onClick.AddListener(OnBackClicked);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(OnPlayClicked);
        controlsButton.onClick.RemoveListener(OnControlsClicked);
        exitButton.onClick.RemoveListener(OnExitClicked);
        backButton.onClick.RemoveListener(OnBackClicked);
    }

    private void Start()
    {
        ShowMainMenu();
    }

    private void OnPlayClicked()
    {
        Debug.Log("[MenuManager] Play button clicked");
        
        SceneManager.LoadScene("Map");
    }

    private void OnControlsClicked()
    {
        Debug.Log("[MenuManager] Controls button clicked");
        ShowControlsPage();
    }

    private void OnExitClicked()
    {
        Debug.Log("[MenuManager] Exit button clicked");
        Application.Quit();
    }

    private void OnBackClicked()
    {
        Debug.Log("[MenuManager] Back button clicked");
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        mainMenuPage.SetActive(true);
        controlsPage.SetActive(false);
        Debug.Log("[MenuManager] Showing Main Menu");
    }

    private void ShowControlsPage()
    {
        mainMenuPage.SetActive(false);
        controlsPage.SetActive(true);
        Debug.Log("[MenuManager] Showing Controls Page");
    }
}


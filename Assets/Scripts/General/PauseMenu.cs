// Pause menu: toggles on the Pause action, freezes time, disables gameplay scripts. Reused by both games.
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private string mainMenuScene = "MainMenu";

    [Tooltip("Scripts disabled while paused — camera look, player movement/combat. Empty is fine for tic-tac-toe.")]
    [SerializeField] private MonoBehaviour[] gameplayScripts;

    public bool IsPaused { get; private set; }

    private void OnEnable()
    {
        pauseAction.action.performed += OnPausePressed;
        pauseAction.action.Enable();
    }

    private void OnDisable()
    {
        pauseAction.action.performed -= OnPausePressed;
        pauseAction.action.Disable();
    }

    private void Start()
    {
        resumeButton.onClick.AddListener(Resume);
        mainMenuButton.onClick.AddListener(() => SceneLoader.Load(mainMenuScene));
        pausePanel.SetActive(false);
    }

    private void OnPausePressed(InputAction.CallbackContext _)
    {
        if (IsPaused)
            Resume();
        else
            Pause();
    }

    // Freeze the game and show the menu.
    public void Pause()
    {
        IsPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        SetGameplayEnabled(false);
    }

    // Unfreeze and hide the menu.
    public void Resume()
    {
        IsPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        SetGameplayEnabled(true);
    }

    private void SetGameplayEnabled(bool value)
    {
        foreach (var script in gameplayScripts)
            if (script)
                script.enabled = value;
    }
}
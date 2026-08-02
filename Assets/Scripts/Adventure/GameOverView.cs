// Shows the win or lose panel and wires "return to main". Observes AdventureGameManager. (View)
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverView : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string winMessage = "You win!";
    [SerializeField] private string loseMessage = "You died";

    [Tooltip("Gameplay scripts to disable when the game ends (camera, movement, combat).")]
    [SerializeField] private MonoBehaviour[] gameplayScripts;

    private void Start()
    {
        panel.SetActive(false);
        mainMenuButton.onClick.AddListener(() => SceneLoader.Load(mainMenuScene));

        var gm = AdventureGameManager.Instance;
        gm.GameWon += OnWin;
        gm.GameLost += OnLose;
    }

    private void OnDestroy()
    {
        if (!AdventureGameManager.Instance)
            return;
        AdventureGameManager.Instance.GameWon -= OnWin;
        AdventureGameManager.Instance.GameLost -= OnLose;
    }

    private void OnWin() => Show(winMessage);
    private void OnLose() => Show(loseMessage);

    // Display the end screen, freeze the world, free the cursor (via disabled camera).
    private void Show(string message)
    {
        messageText.text = message;
        panel.SetActive(true);
        Time.timeScale = 0f;
        foreach (var script in gameplayScripts)
            if (script)
                script.enabled = false;
    }
}
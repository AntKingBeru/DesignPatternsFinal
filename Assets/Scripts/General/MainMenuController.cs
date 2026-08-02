// Main menu: buttons route to each game scene. (Menu controller)
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string ticTacToeScene = "TicTacToe";
    [SerializeField] private string adventureScene = "Adventure";
    [SerializeField] private Button ticTacToeButton;
    [SerializeField] private Button adventureButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        ticTacToeButton.onClick.AddListener(() => SceneLoader.Load(ticTacToeScene));
        adventureButton.onClick.AddListener(() => SceneLoader.Load(adventureScene));
        if (quitButton)
            quitButton.onClick.AddListener(Application.Quit);
    }
}
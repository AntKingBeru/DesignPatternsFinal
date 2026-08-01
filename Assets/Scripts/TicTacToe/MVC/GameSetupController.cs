// Start-of-match screen: Player 1 picks X or O, then the game starts. Player 2 gets the other mark.
using UnityEngine;
using UnityEngine.UI;

public class GameSetupController : MonoBehaviour
{
    [SerializeField] private GameObject setupPanel;
    [SerializeField] private Button chooseXButton;
    [SerializeField] private Button chooseOButton;
    
    private void Start()
    {
        chooseXButton.onClick.AddListener(() => StartWith(Mark.X));
        chooseOButton.onClick.AddListener(() => StartWith(Mark.O));
        setupPanel.SetActive(true);
    }

    private void StartWith(Mark player1Mark)
    {
        GameManager.Instance.StartGame(player1Mark);
        setupPanel.SetActive(false);
    }
}
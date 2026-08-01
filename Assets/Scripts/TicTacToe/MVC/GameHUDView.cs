// Shows score, turn, and result; wires HUD buttons to the GameManager. (View)
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHUDView : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text turnText;
    [SerializeField] private TMP_Text resultText;

    [Header("Buttons")]
    [SerializeField] private Button undoButton;
    [SerializeField] private Button redoButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button replayButton;

    private void Start()
    {
        var gm = GameManager.Instance;
        gm.ScoreChanged += OnScoreChanged;
        gm.TurnChanged += OnTurnChanged;
        gm.GameEnded += OnGameEnded;
        gm.StateChanged += OnStateChanged;
        gm.UndoRedoStateChanged += OnUndoRedoStateChanged;

        // Buttons call the same public methods the input actions do (works on touch too).
        undoButton.onClick.AddListener(gm.Undo);
        redoButton.onClick.AddListener(gm.Redo);
        confirmButton.onClick.AddListener(gm.ConfirmTurn);
        replayButton.onClick.AddListener(gm.BeginRound);

        OnScoreChanged(0, 0);
        if (resultText)
            resultText.gameObject.SetActive(false);
        OnUndoRedoStateChanged();
    }
    
    private void OnDestroy()
    {
        if (!GameManager.Instance)
            return;
        var gm = GameManager.Instance;
        gm.ScoreChanged -= OnScoreChanged;
        gm.TurnChanged -= OnTurnChanged;
        gm.GameEnded -= OnGameEnded;
        gm.StateChanged -= OnStateChanged;
        gm.UndoRedoStateChanged -= OnUndoRedoStateChanged;
    }

    private void OnScoreChanged(int scoreX, int scoreO) => scoreText.text = $"X: {scoreX}   O: {scoreO}";
    private void OnTurnChanged(Mark mark) => turnText.text = $"Turn: {mark}";
    
    private void OnGameEnded(Mark winner)
    {
        if (!resultText)
            return;
        resultText.gameObject.SetActive(true);
        resultText.text = winner == Mark.None ? "Draw!" : $"{winner} wins!";
    }

    private void OnStateChanged(GameState state)
    {
        if (state == GameState.Playing && resultText)
            resultText.gameObject.SetActive(false);
    }

    // Enable each action only when it is currently valid.
    private void OnUndoRedoStateChanged()
    {
        var gm = GameManager.Instance;
        undoButton.interactable = gm.CanUndo;
        redoButton.interactable = gm.CanRedo;
        confirmButton.interactable = gm.CanUndo;
    }
}
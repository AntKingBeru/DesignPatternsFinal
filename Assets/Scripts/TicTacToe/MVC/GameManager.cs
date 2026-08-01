// Central singleton: owns board, turns, score, and the undo/redo history. Coordinates the whole match.
using System;

public enum GameState { Setup, Playing, GameOver }

public class GameManager : Singleton<GameManager>
{
    // Model + command history, created once and reused every round.
    public BoardModel Board { get; private set; }
    private readonly CommandInvoker _invoker = new();
    
    // Turn / score / state.
    public Mark CurrentMark { get; private set; } = Mark.X;
    public GameState State { get; private set; } = GameState.Setup;
    private int _scoreX, _scoreO;
    
    // Player-to-mark mapping chosen at setup (exposed for HUD display).
    public Mark Player1Mark { get; private set; } = Mark.X;
    
    // Events the View layer listens to.
    public event Action<Mark> TurnChanged;
    public event Action<int, int> ScoreChanged;
    public event Action<GameState> StateChanged;
    public event Action<Mark> GameEnded;
    public event Action UndoRedoStateChanged;

    public bool CanUndo => _invoker.CanUndo;
    public bool CanRedo => _invoker.CanRedo;
    
    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
            return;
        Board = new BoardModel();
    }
    
    // Called by the setup UI. player1Mark decides who is X/O; X always moves first.
    public void StartGame(Mark player1Mark)
    {
        Player1Mark = player1Mark;
        _scoreX = _scoreO = 0;
        ScoreChanged?.Invoke(_scoreX, _scoreO);
        BeginRound();
    }
    
    // Resets the board for a new round while keeping the running score.
    public void BeginRound()
    {
        Board.Reset();
        _invoker.Clear();
        CurrentMark = Mark.X;
        SetState(GameState.Playing);
        TurnChanged?.Invoke(CurrentMark);
        UndoRedoStateChanged?.Invoke();
    }
    
    // Tentative placement. Rejected if not playing, cell taken, or a mark is already
    // placed this turn (the player must undo first to move it elsewhere).
    public void TryPlaceMark(int cellIndex)
    {
        if (State != GameState.Playing)
            return;
        if (_invoker.CanUndo)
            return;
        if (!Board.IsEmpty(cellIndex))
            return;

        _invoker.Execute(new PlaceMarkCommand(Board, cellIndex, CurrentMark));
        UndoRedoStateChanged?.Invoke();
    }

    public void Undo()
    {
        if (State != GameState.Playing)
            return;
        _invoker.Undo();
        UndoRedoStateChanged?.Invoke();
    }

    public void Redo()
    {
        if (State != GameState.Playing)
            return;
        _invoker.Redo();
        UndoRedoStateChanged?.Invoke();
    }
    
    // Locks in the move, checks win/draw, then advances the turn.
    public void ConfirmTurn()
    {
        if (State != GameState.Playing)
            return;
        if (!_invoker.CanUndo)
            return;

        _invoker.Clear();

        var winner = WinChecker.GetWinner(Board);
        if (winner != Mark.None)
        {
            AddScore(winner);
            SetState(GameState.GameOver);
            GameEnded?.Invoke(winner);
        }
        else if (Board.IsFull())
        {
            SetState(GameState.GameOver);
            GameEnded?.Invoke(Mark.None);
        }
        else
        {
            CurrentMark = CurrentMark == Mark.X ? Mark.O : Mark.X;
            TurnChanged?.Invoke(CurrentMark);
        }
        UndoRedoStateChanged?.Invoke();
    }

    private void AddScore(Mark mark)
    {
        if (mark == Mark.X)
            _scoreX++;
        else if (mark == Mark.O)
            _scoreO++;
        ScoreChanged?.Invoke(_scoreX, _scoreO);
    }

    private void SetState(GameState newState)
    {
        State = newState;
        StateChanged?.Invoke(newState);
    }
}
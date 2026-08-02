// Places a mark; captures a memento so Undo restores the exact prior state (command + memento hybrid).

public class PlaceMarkCommand : ICommand
{
    private readonly BoardModel _board;
    private readonly int _cellIndex;
    private readonly Mark _mark;
    
    // State captured right before Execute; restored on Undo.
    private BoardMemento _memento;
    
    public PlaceMarkCommand(BoardModel board, int cellIndex, Mark mark)
    {
        _board = board;
        _cellIndex = cellIndex;
        _mark = mark;
    }

    // Snapshot, then place the mark.
    public void Execute()
    {
        _memento = _board.CreateMemento();
        _board.SetCell(_cellIndex, _mark);
    }

    // Restore the board exactly as it was before Execute.
    public void Undo()
    {
        if (_memento != null)
            _board.RestoreMemento(_memento);
    }
}
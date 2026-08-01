// Hold the 3x3 grid state and raises events on change. Pure logic — no Unity rendering.
using System;

public enum Mark
{
    None,
    X,
    O
}

public class BoardModel
{
    public const int Size = 3;
    public const int CellCount = Size * Size;
    
    private readonly Mark[] _cells = new Mark[CellCount];
    
    // Views subscribe to these to update visuals
    public event Action<int, Mark> CellChanged; // a single cell changed
    public event Action BoardReset; // whole board cleared
    
    public Mark GetCell(int index) => _cells[index];
    public bool IsEmpty(int index) => _cells[index] == Mark.None;
    
    // Sets a cell and notifies listeners.
    public void SetCell(int index, Mark mark)
    {
        _cells[index] = mark;
        CellChanged?.Invoke(index, mark);
    }
    
    // Clears every cell and notifies listeners.
    public void Reset()
    {
        for (var i = 0; i < CellCount; i++)
            _cells[i] = Mark.None;
        BoardReset?.Invoke();
    }
    
    // True when no cell is empty — used for draw detection.
    public bool IsFull()
    {
        for (var i = 0; i < CellCount; i++)
            if (_cells[i] == Mark.None)
                return false;
        return true;
    }

    public BoardMemento CreateMemento() => new(_cells);
    
    // Restores a snapshot and refreshes all listeners so the view syncs.
    public void RestoreMemento(BoardMemento memento)
    {
        var state = memento.GetState();
        Array.Copy(state, _cells, CellCount);
        for (var i = 0; i < CellCount; i++)
            CellChanged?.Invoke(i, state[i]);
    }
    
    // Coordinate helpers
    public static int ToIndex(int col, int row) => row * Size + col;
    public static int ColOf(int index) => index % Size;
    public static int RowOf(int index) => index / Size;
}
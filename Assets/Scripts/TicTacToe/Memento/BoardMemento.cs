// Immutable snapshot of the board, used by the memento pattern to support undo/redo.

public class BoardMemento
{
    // Copy of the grid taken at capture time; only BoardModel restores it.
    private readonly Mark[] _state;

    public BoardMemento(Mark[] state) => _state = (Mark[])state.Clone();

    // Returns a defensive copy so callers can't mutate the saved state.
    public Mark[] GetState() => (Mark[])_state.Clone();
}
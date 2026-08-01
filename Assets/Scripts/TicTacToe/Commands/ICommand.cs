// Contract for an undoable action

public interface ICommand
{
    void Execute(); // perform the action
    void Undo(); // reverse the action
}
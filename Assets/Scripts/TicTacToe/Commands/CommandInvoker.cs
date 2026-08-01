// Runs commands and keeps the undo/redo stacks (command pattern's "invoker").
using System.Collections.Generic;

public class CommandInvoker
{
    private readonly Stack<ICommand> _undoStack = new();
    private readonly Stack<ICommand> _redoStack = new();
    
    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;
    
    // Run a fresh command; any pending redo history is invalidated.
    public void Execute(ICommand command)
    {
        command.Execute();
        _undoStack.Push(command);
        _redoStack.Clear();
    }

    public void Undo()
    {
        if (!CanUndo)
            return;
        var command = _undoStack.Pop();
        command.Undo();
        _redoStack.Push(command);
    }

    public void Redo()
    {
        if (!CanRedo)
            return;
        var command = _redoStack.Pop();
        command.Execute();
        _undoStack.Push(command);
    }

    // Wipe history — called when a turn is confirmed or a new round starts.
    public void Clear()
    {
        _undoStack.Clear();
        _redoStack.Clear();
    }
}
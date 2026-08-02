// Subject of the observer pattern: holds all trackable objects and notifies observers (the minimap) of changes.
using System.Collections.Generic;

public class MinimapRegistry : Singleton<MinimapRegistry>
{
    // Live world objects to draw, and the observers watching them.
    private readonly List<ITrackable> _trackables = new();
    private readonly List<IMinimapObserver> _observers = new();
    
    // Subscribe an observer and replay current trackable objects so it starts in sync (handles load-order).
    public void AddObserver(IMinimapObserver observer)
    {
        if (_observers.Contains(observer))
            return;
        _observers.Add(observer);
        foreach (var trackable in _trackables)
            observer.OnTrackableAdded(trackable);
    }
    
    public void RemoveObserver(IMinimapObserver observer) => _observers.Remove(observer);
    
    // Add a trackable and notify every observer.
    public void Register(ITrackable trackable)
    {
        if (_trackables.Contains(trackable)) return;
        _trackables.Add(trackable);
        foreach (var observer in _observers)
            observer.OnTrackableAdded(trackable);
    }

    // Remove a trackable and notify every observer (drives blip removal on despawn).
    public void Unregister(ITrackable trackable)
    {
        if (!_trackables.Remove(trackable)) return;
        foreach (var observer in _observers)
            observer.OnTrackableRemoved(trackable);
    }
}
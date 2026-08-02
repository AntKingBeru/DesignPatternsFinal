// Observer contract: notified when a trackable enter or leave the world.
public interface IMinimapObserver
{
    void OnTrackableAdded(ITrackable trackable);
    void OnTrackableRemoved(ITrackable trackable);
}
// Singleton that tracks the score; observes pickup-collected events and raises ScoreChanged for the UI.
using System;

public class ScoreManager : Singleton<ScoreManager>
{
    public int Score { get; private set; }
    
    // View layer (ScoreView) observes this to refresh the label.
    public event Action<int> ScoreChanged;
    
    // Subscribe to the pickup subject only on the real instance (base.Awake set Instance first).
    private void OnEnable()
    {
        if (Instance == this)
            PickupEvents.Collected += OnPickupCollected;
    }

    // Always unsubscribe -> no dangling reference on a static event.
    private void OnDisable() => PickupEvents.Collected -= OnPickupCollected;

    // Observer callback: add the pickup's value and notify the UI.
    private void OnPickupCollected(int value)
    {
        Score += value;
        ScoreChanged?.Invoke(Score);
    }
}
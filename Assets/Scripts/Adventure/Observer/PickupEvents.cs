// Broadcasts pickup collection to any interested system (observer pattern via an event aggregator).
using System;

public static class PickupEvents
{
    // Fired when any pickup is collected; carries the score value. ScoreManager subscribes.
    public static event Action<int> Collected;
    public static event Action Spawned;

    public static void RaiseCollected(int scoreValue) => Collected?.Invoke(scoreValue);
    public static void RaiseSpawned() => Spawned?.Invoke();
}
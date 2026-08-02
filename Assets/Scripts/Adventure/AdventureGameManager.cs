// Adventure win/lose rules: win when all pickups are collected, lose when the player dies. (Singleton controller)
using System;
using UnityEngine;

public class AdventureGameManager : Singleton<AdventureGameManager>
{
    [SerializeField] private string playerTag = "Player";
    
    private int _totalPickups;
    private int _collectedPickups;
    private bool _gameOver;

    public event Action GameWon;
    public event Action GameLost;
    
    // Subscribe only on the real instance (base.Awake set Instance first).
    private void OnEnable()
    {
        if (Instance != this)
            return;
        PickupEvents.Spawned += OnPickupSpawned;
        PickupEvents.Collected += OnPickupCollected;
    }

    private void OnDisable()
    {
        PickupEvents.Spawned -= OnPickupSpawned;
        PickupEvents.Collected -= OnPickupCollected;
    }

    // Hook the player's death after spawners have populated the scene.
    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag(playerTag);
        if (player && player.TryGetComponent(out Health health))
            health.Died += OnPlayerDied;
    }

    private void OnPickupSpawned() => _totalPickups++;

    private void OnPickupCollected(int _)
    {
        if (_gameOver)
            return;
        _collectedPickups++;
        if (_collectedPickups >= _totalPickups && _totalPickups > 0)
        {
            _gameOver = true;
            GameWon?.Invoke();
        }
    }

    private void OnPlayerDied()
    {
        if (_gameOver)
            return;
        _gameOver = true;
        GameLost?.Invoke();
    }
}
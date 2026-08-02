// Spawns a set number of prefabs at random points in a box area at start. (World population)
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int count = 8;
    [SerializeField] private Vector2 areaSize = new(40f, 40f);
    [SerializeField] private float spawnHeight = 0.5f;
    
    [Header("Overlap Avoidance")]
    [SerializeField] private float clearRadius = 1.5f;
    [SerializeField] private LayerMask blockingMask;
    [SerializeField] private int maxAttemptsPerItem = 15;
    
    // Populate the world once; each spawned Trackable self-registers with the minimap.
    private void Start()
    {
        for (var i = 0; i < count; i++)
        {
            if (TryFindClearPoint(out var point))
                Instantiate(prefab, point, Quaternion.identity);
        }
    }
    
    // Sample random points until one has no blocking colliders within clearRadius.
    private bool TryFindClearPoint(out Vector3 point)
    {
        for (var attempt = 0; attempt < maxAttemptsPerItem; attempt++)
        {
            var candidate = transform.position + new Vector3(
                Random.Range(-areaSize.x * 0.5f, areaSize.x * 0.5f),
                spawnHeight,
                Random.Range(-areaSize.y * 0.5f, areaSize.y * 0.5f));

            if (!Physics.CheckSphere(candidate, clearRadius, blockingMask))
            {
                point = candidate;
                return true;
            }
        }
        point = Vector3.zero;
        return false;
    }
    
    // Draw the spawn area in the editor so you can size it visually.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + Vector3.up * spawnHeight,
            new Vector3(areaSize.x, 0.1f, areaSize.y));
    }
}
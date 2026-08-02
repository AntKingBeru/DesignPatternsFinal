// Observer of MinimapRegistry: creates, colors, and removes minimap blips for each trackable. (View)
using System.Collections.Generic;
using UnityEngine;

public class MinimapView : MonoBehaviour, IMinimapObserver
{
    [Header("References")]
    [SerializeField] private RectTransform blipRoot;
    [SerializeField] private MinimapBlip blipPrefab;
    [SerializeField] private Transform player;

    [Header("Scale")]
    [SerializeField] private float worldRadius = 40f;
    [SerializeField] private float mapPixelRadius = 90f;

    [Header("Blip Colors")]
    [SerializeField] private Color playerColor = Color.white;
    [SerializeField] private Color enemyColor = Color.red;
    [SerializeField] private Color pickupColor = Color.yellow;

    // Active blips keyed by trackable for O(1) removal.
    private readonly Dictionary<ITrackable, MinimapBlip> _blips = new();

    // Subscribe as observer; AddObserver replays existing trackables so we don't miss any.
    private void OnEnable()
    {
        if (MinimapRegistry.Instance)
            MinimapRegistry.Instance.AddObserver(this);
    }

    private void OnDisable()
    {
        if (MinimapRegistry.Instance)
            MinimapRegistry.Instance.RemoveObserver(this);
    }

    // Observer callback: spawn a blip for a new trackable.
    public void OnTrackableAdded(ITrackable trackable)
    {
        if (_blips.ContainsKey(trackable)) return;
        var blip = Instantiate(blipPrefab, blipRoot);
        blip.Initialize(trackable, player, mapPixelRadius / worldRadius, mapPixelRadius, ColorFor(trackable.Type));
        _blips.Add(trackable, blip);
    }

    // Observer callback: remove the blip when its trackable despawns.
    public void OnTrackableRemoved(ITrackable trackable)
    {
        if (!_blips.TryGetValue(trackable, out var blip))
            return;
        if (blip) Destroy(blip.gameObject);
        _blips.Remove(trackable);
    }

    private Color ColorFor(TrackableType type)
    {
        return type switch
        {
            TrackableType.Player => playerColor,
            TrackableType.Enemy => enemyColor,
            _ => pickupColor
        };
    }
}
// Registers/unregisters this object with the MinimapRegistry so it shows on the minimap. (Subject participant)
using UnityEngine;

public class Trackable : MonoBehaviour, ITrackable
{
    [SerializeField] private TrackableType type;

    public Transform Transform => transform;
    public TrackableType Type => type;

    // Register when active -> registry tells the minimap to add a blip.
    private void OnEnable()
    {
        if (MinimapRegistry.Instance)
            MinimapRegistry.Instance.Register(this);
    }

    // Unregister on disable/destroy -> minimap removes the blip. This is how despawn updates the map.
    private void OnDisable()
    {
        if (MinimapRegistry.Instance)
            MinimapRegistry.Instance.Unregister(this);
    }
}
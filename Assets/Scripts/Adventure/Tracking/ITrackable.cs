// Anything the minimap can display: exposes its world transform and category.
using UnityEngine;

public enum TrackableType { Player, Enemy, Pickup }

public interface ITrackable
{
    Transform Transform { get; }
    TrackableType Type { get; }
}
// A collectible: on player contact it awards points (via the subject) and despawns, updating the minimap.
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Pickup : MonoBehaviour
{
    [SerializeField] private int scoreValue = 1;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private Collider col;

    private bool _collected;

    // Make the collider a trigger by default when the component is added.
    private void Reset() => col.isTrigger = true;
    
    private void Start() => PickupEvents.RaiseSpawned();

    // On player entry: notify observers, then destroy (Trackable.OnDisable removes the blip).
    private void OnTriggerEnter(Collider other)
    {
        if (_collected || !other.CompareTag(playerTag))
            return;
        _collected = true;
        PickupEvents.RaiseCollected(scoreValue);
        Destroy(gameObject);
    }
}
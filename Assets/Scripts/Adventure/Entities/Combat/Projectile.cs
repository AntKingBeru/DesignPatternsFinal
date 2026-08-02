// Flies forward and damages the first hittable Health it touches, then despawns. (Enemy shots + player ranged)
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 4f;
    [SerializeField] private Rigidbody rb;

    private LayerMask _hittableMask;

    // Set by the firer so the shot only hurts the intended faction.
    public void Configure(LayerMask mask) => _hittableMask = mask;
    
    private void Start() => Destroy(gameObject, lifetime);

    // Kinematic forward motion so trigger events fire cleanly.
    private void FixedUpdate() => rb.MovePosition(transform.position + transform.forward * (speed * Time.fixedDeltaTime));

    private void OnTriggerEnter(Collider other)
    {
        if ((_hittableMask.value & (1 << other.gameObject.layer)) == 0)
            return;
        var health = other.GetComponentInParent<Health>();
        if (health)
            health.TakeDamage(damage, transform.position);
        Destroy(gameObject);
    }
}
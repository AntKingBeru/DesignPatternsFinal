// Applies a decaying knockback impulse through the CharacterController when Health takes damage. (Feedback)
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Knockback : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private float force = 6f;
    [SerializeField] private float decay = 12f;
    [SerializeField] private CharacterController controller;
    
    private Vector3 _velocity;

    private void OnEnable()
    {
        if (health)
            health.Damaged += OnDamaged;
    }

    private void OnDisable()
    {
        if (health)
            health.Damaged -= OnDamaged;
    }

    // Kick off a push in the hit direction (away from the source).
    private void OnDamaged(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.0001f)
            _velocity = direction * force;
    }

    // Apply and decay the impulse each frame.
    private void Update()
    {
        if (_velocity.sqrMagnitude < 0.01f)
        {
            _velocity = Vector3.zero;
            return;
        }
        controller.Move(_velocity * Time.deltaTime);
        _velocity = Vector3.MoveTowards(_velocity, Vector3.zero, decay * Time.deltaTime);
    }
}
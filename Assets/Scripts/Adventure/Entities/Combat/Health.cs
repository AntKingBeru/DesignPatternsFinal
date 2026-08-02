// Tracks hit points; applies damage and raises events. Shared by player and enemies.
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    public int Current { get; private set; }

    public event Action<Vector3> Damaged;
    public event Action<int, int> HealthChanged;
    public event Action Died;

    private void Awake() => Current = maxHealth;

    // No source given -> flash but no directional knockback.
    public void TakeDamage(int amount) => TakeDamage(amount, transform.position);

    // Source given -> knockback pushes away from it.
    public void TakeDamage(int amount, Vector3 sourcePosition)
    {
        if (Current <= 0)
            return;
        Current = Mathf.Max(0, Current - amount);

        var dir = transform.position - sourcePosition;
        dir.y = 0f;
        dir = dir.sqrMagnitude > 0.0001f ? dir.normalized : Vector3.zero;

        Damaged?.Invoke(dir);
        HealthChanged?.Invoke(Current, maxHealth);
        if (Current == 0)
            Died?.Invoke();
    }
}
// Melee slash: shows a slash visual and damages Health in a forward arc within range. (Player + melee enemy)
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private float range = 2.5f;
    [SerializeField] private float arcAngle = 120f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float cooldown = 0.6f;
    [SerializeField] private LayerMask hittableMask;
    [SerializeField] private SlashVisual slashPrefab;
    [SerializeField] private Transform origin;

    private float _nextReadyTime;
    private readonly HashSet<Health> _hitThisSwing = new();

    public float Range => range;
    public bool IsReady => Time.time >= _nextReadyTime;

    // Attempt an attack; returns true if it fired. Called by player input or enemy AI.
    public bool TryAttack()
    {
        if (!IsReady)
            return false;
        _nextReadyTime = Time.time + cooldown;
        Perform();
        return true;
    }

    private void Perform()
    {
        var orig = origin ? origin : transform;
        if (slashPrefab)
            Instantiate(slashPrefab, orig.position, orig.rotation).Play(range, arcAngle);

        _hitThisSwing.Clear();
        var hits = Physics.OverlapSphere(orig.position, range, hittableMask);
        foreach (var hit in hits)
        {
            var toTarget = hit.transform.position - orig.position;
            toTarget.y = 0f;
            if (toTarget.sqrMagnitude < 0.0001f)
                continue;
            if (Vector3.Angle(orig.forward, toTarget) > arcAngle * 0.5f)
                continue;

            var health = hit.GetComponentInParent<Health>();
            if (health && _hitThisSwing.Add(health))
                health.TakeDamage(damage, orig.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((origin ? origin : transform).position, range);
    }
}
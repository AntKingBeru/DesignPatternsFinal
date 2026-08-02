// Melee enemy brain: slash when the player is within reach (movement handles closing the gap).
using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(MeleeAttack))]
public class MeleeEnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyMovement movement;
    [SerializeField] private MeleeAttack melee;

    private void Update()
    {
        if (!movement.Target)
            return;
        if (movement.DistanceToTarget <= melee.Range)
            melee.TryAttack();
    }
}
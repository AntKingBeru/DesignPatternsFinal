// Ranged enemy brain: face the player and shoot when within firing range.
using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(RangedAttack))]
public class RangedEnemyAI : MonoBehaviour
{
    [SerializeField] private float fireRange = 12f;
    [SerializeField] private EnemyMovement movement;
    [SerializeField] private RangedAttack ranged;

    private void Update()
    {
        if (!movement.Target || movement.DistanceToTarget > fireRange)
            return;
        movement.FaceTarget();
        ranged.TryFire();
    }
}
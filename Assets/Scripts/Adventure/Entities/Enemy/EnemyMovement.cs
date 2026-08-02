// Enemy locomotion: gravity + chase the player within aggro range, stopping at a set distance. (Movement only)
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float aggroRange = 12f;
    [SerializeField] private float stoppingDistance = 1.8f;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private CharacterController controller;
    
    private float _verticalVelocity;
    
    // Read by the AI brains to decide when to attack.
    public Transform Target { get; private set; }
    public float DistanceToTarget { get; private set; } = Mathf.Infinity;
    
    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag(playerTag);
        if (player)
            Target = player.transform;
    }
    
    // Chase logic + gravity, applied through the controller each frame.
    private void Update()
    {
        var move = Vector3.zero;
        if (Target)
        {
            var flat = Target.position - transform.position;
            flat.y = 0f;
            DistanceToTarget = flat.magnitude;

            if (DistanceToTarget <= aggroRange && DistanceToTarget > stoppingDistance)
            {
                var dir = flat.normalized;
                move = dir * moveSpeed;
                FaceDirection(dir);
            }
            else if (DistanceToTarget <= aggroRange)
            {
                FaceDirection(flat.normalized);
            }
        }

        _verticalVelocity = controller.isGrounded ? -1f : _verticalVelocity + gravity * Time.deltaTime;
        controller.Move((move + Vector3.up * _verticalVelocity) * Time.deltaTime);
    }

    private void FaceDirection(Vector3 dir)
    {
        if (dir.sqrMagnitude < 0.0001f)
            return;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir),
            turnSpeed * Time.deltaTime);
    }

    // Public so a ranged brain can ensure it's aimed before firing.
    public void FaceTarget()
    {
        if (!Target)
            return;
        var flat = Target.position - transform.position; flat.y = 0f;
        FaceDirection(flat.normalized);
    }
}
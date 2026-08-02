// Camera-relative third-person movement using the new Input System. (Movement only)
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float gravity = -20f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CharacterController controller;
    
    private float _verticalVelocity;
    
    private void OnEnable() => moveAction.action.Enable();
    private void OnDisable() => moveAction.action.Disable();

    // Face camera yaw, move relative to camera, apply gravity.
    private void Update()
    {
        if (!cameraTransform)
            return;

        // Camera forward/right flattened onto the ground plane.
        var forward = cameraTransform.forward;
        forward.y = 0f; forward.Normalize();
        var right = cameraTransform.right;
        right.y = 0f; right.Normalize();

        // Body always points where the camera looks — no rotating toward the move vector.
        if (forward.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(forward);

        var input = moveAction.action.ReadValue<Vector2>();
        var move = Vector3.ClampMagnitude(forward * input.y + right * input.x, 1f) * moveSpeed;

        _verticalVelocity = controller.isGrounded ? -1f : _verticalVelocity + gravity * Time.deltaTime;
        controller.Move((move + Vector3.up * _verticalVelocity) * Time.deltaTime);
    }
}
// Third-person orbit camera: Look input rotates yaw/pitch around the player, over-the-shoulder. (Camera only)
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private InputActionReference lookAction;
    
    [Header("Look")]
    [SerializeField] private float sensitivity = 0.12f;
    [SerializeField] private float minPitch = -25f;
    [SerializeField] private float maxPitch = 65f;
    
    [Header("Rig")]
    [SerializeField] private float distance = 4f;
    [SerializeField] private float height = 1.6f;
    [SerializeField] private float shoulder = 0.7f;
    [SerializeField] private float followSmooth = 0.05f;
    
    private float _yaw, _pitch;
    private Vector3 _velocity;
    
    // Lock the cursor while playing; release it when disabled.
    private void OnEnable()
    {
        lookAction.action.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        lookAction.action.Disable();
        Cursor.lockState = CursorLockMode.None;
    }
    
    // Accumulate look into yaw/pitch, then place the camera behind an offset focus point.
    private void LateUpdate()
    {
        if (!target)
            return;

        var look = lookAction.action.ReadValue<Vector2>();
        _yaw += look.x * sensitivity;
        _pitch = Mathf.Clamp(_pitch - look.y * sensitivity, minPitch, maxPitch);

        var rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        var focus = target.position + Vector3.up * height + (rotation * Vector3.right) * shoulder;
        var desired = focus - (rotation * Vector3.forward) * distance;

        transform.position = Vector3.SmoothDamp(transform.position, desired, ref _velocity, followSmooth);
        transform.rotation = rotation;
    }
}

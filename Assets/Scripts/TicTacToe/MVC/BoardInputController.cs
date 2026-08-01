// Turns board clicks into placement requests using the new Input System. Controller layer.
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardInputController : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference clickAction;
    [SerializeField] private InputActionReference pointAction;
    
    [Header("Scene References")]
    [SerializeField] private Camera boardCamera;
    [SerializeField] private BoardLayout layout;
    
    private void OnEnable()
    {
        clickAction.action.performed += OnClick;
        clickAction.action.Enable();
        pointAction.action.Enable();
    }

    private void OnDisable()
    {
        clickAction.action.performed -= OnClick;
        clickAction.action.Disable();
        pointAction.action.Disable();
    }
    
    // Read pointer -> world -> board index, then ask the GameManager to place.
    private void OnClick(InputAction.CallbackContext context)
    {
        var screen = pointAction.action.ReadValue<Vector2>();
        var world = boardCamera.ScreenToWorldPoint(new Vector3(screen.x, screen.y, 0f));
        if (layout.TryWorldToIndex(world, out var index))
            GameManager.Instance.TryPlaceMark(index);
    }
}
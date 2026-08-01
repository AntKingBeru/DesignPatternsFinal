// Binds undo / redo / confirm input actions to the GameManager. Kept separate from board input (single responsibility).
using UnityEngine;
using UnityEngine.InputSystem;

public class GameActionsInputController : MonoBehaviour
{
    [SerializeField] private InputActionReference undoAction;
    [SerializeField] private InputActionReference redoAction;
    [SerializeField] private InputActionReference confirmAction;
    
    private void OnEnable()
    {
        undoAction.action.performed += OnUndo;
        redoAction.action.performed += OnRedo;
        confirmAction.action.performed += OnConfirm;
        undoAction.action.Enable();
        redoAction.action.Enable();
        confirmAction.action.Enable();
    }

    private void OnDisable()
    {
        undoAction.action.performed -= OnUndo;
        redoAction.action.performed -= OnRedo;
        confirmAction.action.performed -= OnConfirm;
        undoAction.action.Disable();
        redoAction.action.Disable();
        confirmAction.action.Disable();
    }
    
    private void OnUndo(InputAction.CallbackContext _) => GameManager.Instance.Undo();
    private void OnRedo(InputAction.CallbackContext _) => GameManager.Instance.Redo();
    private void OnConfirm(InputAction.CallbackContext _) => GameManager.Instance.ConfirmTurn();
}
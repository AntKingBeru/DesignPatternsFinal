// Routes player attack inputs to the melee slash and the ranged slash-shot. (Player combat input)
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private InputActionReference meleeAction;
    [SerializeField] private InputActionReference rangedAction;
    [SerializeField] private MeleeAttack melee;
    [SerializeField] private RangedAttack ranged;

    private void OnEnable()
    {
        meleeAction.action.performed += OnMelee;
        rangedAction.action.performed += OnRanged;
        meleeAction.action.Enable();
        rangedAction.action.Enable();
    }

    private void OnDisable()
    {
        meleeAction.action.performed -= OnMelee;
        rangedAction.action.performed -= OnRanged;
        meleeAction.action.Disable();
        rangedAction.action.Disable();
    }

    private void OnMelee(InputAction.CallbackContext _) => melee.TryAttack();
    private void OnRanged(InputAction.CallbackContext _) => ranged.TryFire();
}
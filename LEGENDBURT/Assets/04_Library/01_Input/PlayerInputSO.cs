using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "Library/Input/PlayerInputSO")]
public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
{
    public Vector2 MoveInput { get; private set; }
    public bool IsDrifting { get; private set; }

    public event Action<Vector2> OnMoveChanged;
    public event Action<bool> OnDriftChanged;
    public event Action OnBoostPressed;

    public event Action OnPartsActivePressed_01;
    public event Action OnPartsActivePressed_02;

    private Controls _controls;

    private void OnEnable()
    {
        _controls ??= new Controls();
        _controls.Player.SetCallbacks(this);
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls?.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
        OnMoveChanged?.Invoke(MoveInput);
    }

    public void OnDrift(InputAction.CallbackContext context)
    {
        IsDrifting = context.phase == InputActionPhase.Performed;
        OnDriftChanged?.Invoke(IsDrifting);
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnBoostPressed?.Invoke();
    }

    public void OnPartsActive_01(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnPartsActivePressed_01?.Invoke();
    }

    public void OnPartsActive_02(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnPartsActivePressed_02?.Invoke();
    }
}
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : ModuleOwner
{
    [field:SerializeField]public Vector2 MoveDir { get; private set; }
    [field:SerializeField]public BoostModule BoostEffectModule  { get; private set; }
    [SerializeField] private PlayerInputSO InputSO;
    public Rigidbody Rigid { get; private set; }
    public bool IsDrifting { get; private set; }

    protected override void InitializeComponents()
    {
        base.InitializeComponents();
        InputSO.OnMoveChanged += HandleMoveChanged;
        InputSO.OnDriftChanged += HandleDriftChanged;
        InputSO.OnShiftPressed += HandleShiftPressed;
        BoostEffectModule = GetModule<BoostModule>();
        Rigid = GetComponent<Rigidbody>();
    }

    private void HandleShiftPressed()
    {
        BoostEffectModule.Activate();
    }

    private void HandleMoveChanged(Vector2 vector)
    {
        MoveDir = vector;
    }

    private void HandleDriftChanged(bool obj)
    {
        IsDrifting = obj;
    }
}

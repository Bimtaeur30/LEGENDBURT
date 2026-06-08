using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : ModuleOwner
{
    [Header("TEST")]
    [field:SerializeField]public EventChannelSO PlayerChannel { get; private set; }
    [SerializeField] private PartBase TestParts;
    [SerializeField] private ArtifactSO TestArtifactSO;
    [SerializeField] private ArtifactSO TestArtifactSO_2;

    [Header("Input")]
    [field:SerializeField]public Vector2 MoveDir { get; private set; }
    [SerializeField] private PlayerInputSO InputSO;

    public MovementModule MovementModule { get; private set; }

    public Rigidbody Rigid { get; private set; }
    public bool IsDrifting { get; private set; }

    protected override void InitializeComponents()
    {
        base.InitializeComponents();
        InputSO.OnMoveChanged += HandleMoveChanged;
        InputSO.OnDriftChanged += HandleDriftChanged;
        InputSO.OnBoostPressed += HandleBoostPressed;

        InputSO.OnPartsActivePressed_01 += HandleOnPartsActivePressed_01;
        InputSO.OnPartsActivePressed_02 += HandleOnPartsActivePressed_02;

        Rigid = GetComponent<Rigidbody>();
        MovementModule = GetModule<MovementModule>();
    }


    private void Start()
    {
        // Å×½ºÆ® ÆÄÃ÷ ºÎÂø ÄÚµå
        //PlayerChannel.RasiseEvent(PlayerEvents.RemovePartsEvent.Init(PartsJointPos.FirstSlot));
        //PlayerChannel.RasiseEvent(PlayerEvents.RemovePartsEvent.Init(PartsJointPos.SecondSlot));
        //PlayerChannel.RasiseEvent(PlayerEvents.AttachPartsEvent.Init(TestParts, PartsJointPos.FirstSlot));
        //PlayerChannel.RasiseEvent(PlayerEvents.AttachPartsEvent.Init(TestParts, PartsJointPos.SecondSlot));
        PlayerChannel.RasiseEvent(PlayerEvents.OnCardSelectEvent);

        // Å×½ºÆ® ÆÄÃ÷ ºÎÂø ÄÚµå ³¡
    }
    private void Update()
    {
        if (Keyboard.current.f1Key.wasPressedThisFrame)
            PlayerChannel.RasiseEvent(PlayerEvents.OnCardSelectEvent); // Å×½ºÆ® ÆÄÃ÷ È¹µæ
        if (Keyboard.current.f2Key.wasPressedThisFrame)
            PlayerChannel.RasiseEvent(PlayerEvents.EquipItemEvent.Init(TestArtifactSO)); // Å×½ºÆ® À¯¹° È¹µæ
        if (Keyboard.current.f3Key.wasPressedThisFrame)
            PlayerChannel.RasiseEvent(PlayerEvents.EquipItemEvent.Init(TestArtifactSO_2)); // Å×½ºÆ® À¯¹° È¹µæ
    }
    private void HandleBoostPressed()
    {
        //BoostEffectModule.Activate();
    }

    private void HandleOnPartsActivePressed_01()
    {
        //PartsModule.ActivateParts(PartsJointPos.FirstSlot);
        PlayerChannel.RasiseEvent(PlayerEvents.ActivePartsEvent.Init(PartsJointPos.FirstSlot));
    }
    private void HandleOnPartsActivePressed_02()
    {
        //PartsModule.ActivateParts(PartsJointPos.SecondSlot);
        PlayerChannel.RasiseEvent(PlayerEvents.ActivePartsEvent.Init(PartsJointPos.SecondSlot));
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

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
    public ChatModule ChatModule { get; private set; }

    public Rigidbody Rigid { get; private set; }
    public bool IsDrifting { get; private set; }
    private bool isMovementInputEnabled = true;


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
        ChatModule = GetModule<ChatModule>();

        PlayerChannel.AddListener<SetActivePlayerMovementInputEvent>(HandleSetActivePlayerMovementInputEvent);
        PlayerChannel.AddListener<OnGameOverEvent>(HandleOnGameOverEvent);
        PlayerChannel.AddListener<OnGameStartEvent>(HandleOnGameStartEvent);
    }


    private void OnDestroy()
    {
        PlayerChannel.RemoveListener<SetActivePlayerMovementInputEvent>(HandleSetActivePlayerMovementInputEvent);
        PlayerChannel.RemoveListener<OnGameOverEvent>(HandleOnGameOverEvent);
        PlayerChannel.RemoveListener<OnGameStartEvent>(HandleOnGameStartEvent);
    }
    private void HandleSetActivePlayerMovementInputEvent(SetActivePlayerMovementInputEvent @event)
    {
        ToggleMovementInput(@event.IsActive);
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
            PlayerChannel.RasiseEvent(PlayerEvents.OnItemSelectEvent);
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
        if (!isMovementInputEnabled) return;
        MoveDir = vector;
    }

    private void HandleDriftChanged(bool obj)
    {
        if (!isMovementInputEnabled) return;
        if (obj == true)
            GameOverManager.Instance.DriftCount++;
        IsDrifting = obj;
    }
    private void ToggleMovementInput(bool active)
    {
        isMovementInputEnabled = active;
        MoveDir = Vector3.zero;
        IsDrifting = false;
        //Rigid.linearVelocity = Vector3.zero;
    }

    private void HandleOnGameOverEvent(OnGameOverEvent @event)
    {
        if (@event.IsGameSuccess)
        {
            ChatModule.GenerateChat("ÈÞ »ì¾Ò´Ù");
        }
        else
        {
            ChatModule.GenerateChat("¤Ð¤Ð¤Ð");
        }
    }
    private void HandleOnGameStartEvent(OnGameStartEvent @event)
    {
        ChatModule.GenerateChat("±Þ¶ËÀÌ´Ù!!");
    }
}

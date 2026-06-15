using System;
using System.Collections.Generic;
using Assets._02_Scripts._01_System.Stage;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class StageManager : MonoSingleton<StageManager>
{
    [field:SerializeField] public StageDataSO[] stageData { get; private set; }
    [SerializeField] private EventChannelSO playerChannel;
    [SerializeField] private EventChannelSO stageChannel;

    [field: SerializeField] public int CurrentStageIndex { get; private set; } = -1; // 테스트로 0 해뒀음, 실제로는 로비에서는 -1상태, 스테이지 1 시작 시 0을 보장해야함.

    public StageDataSO CurrentStageData { get; private set; } = null;
    public PartsDataSO Save_parts1 { get; private set; } = null;
    public PartsDataSO Save_parts2 { get; private set; } = null;
    public List<ArtifactSO> Save_artifactSOs { get; private set; } = new();

    protected override void Awake()
    {
        base.Awake();
        playerChannel.AddListener<AttachPartsEvent>(HandleAttachPartsEvent);
        //playerChannel.AddListener<EquipItemEvent>(HandleEquipItemEvent);

        stageChannel.AddListener<MoveNextStageEvent>(HandleMoveNextStageEvent);
        stageChannel.AddListener<CreateStageSaveDataEvent>(HandleCreateStageSaveDataEvent);
        stageChannel.AddListener<RemoveStageSaveDataEvent>(HandleRemoveStageSaveDataEvent);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (CurrentStageIndex >= 0)
        {
            InitializeStage();
        }
    }

    private void OnDestroy()
    {
        playerChannel.RemoveListener<AttachPartsEvent>(HandleAttachPartsEvent);
        //playerChannel.RemoveListener<EquipItemEvent>(HandleEquipItemEvent);

        stageChannel.RemoveListener<MoveNextStageEvent>(HandleMoveNextStageEvent);
        stageChannel.RemoveListener<CreateStageSaveDataEvent>(HandleCreateStageSaveDataEvent);
        stageChannel.RemoveListener<RemoveStageSaveDataEvent>(HandleRemoveStageSaveDataEvent);
    }

    private void InitializeStage()
    {
        CurrentStageData = stageData[CurrentStageIndex];
        Stage stage = Instantiate(CurrentStageData.StagePrefab, Vector3.zero, Quaternion.identity);
        stage.Initialize();
    }

    private void HandleAttachPartsEvent(AttachPartsEvent @event)
    {
        switch(@event.JointPos)
        {
            case PartsJointPos.FirstSlot:
                Save_parts1 = @event.Parts;
                break;
            case PartsJointPos.SecondSlot:
                Save_parts2 = @event.Parts;
                break;
        }
    }

    private void HandleMoveNextStageEvent(MoveNextStageEvent @event)
    {
        if (CurrentStageIndex >= stageData.Length - 1)
        {
            CurrentStageIndex = -1;
            Save_parts1 = null;
            Save_parts2 = null;
            Save_artifactSOs.Clear();
            SceneManager.LoadScene("02_Ending"); // 엔딩씬으로 이동
        }
        else
        {
            CurrentStageIndex++;
            Save_parts1 = @event.FirstParts;
            Save_parts2 = @event.SecondParts;
            Save_artifactSOs = @event.ArtifactLIst;

            SceneManager.LoadScene("01_MainGame");
        }
    }
    private void HandleCreateStageSaveDataEvent(CreateStageSaveDataEvent @event)
    {
        CurrentStageIndex = 0;
    }
    private void HandleRemoveStageSaveDataEvent(RemoveStageSaveDataEvent @event)
    {
        CurrentStageIndex = -1; // 테스트로 0 해둔거임, -1로 고쳐야함
        Save_parts1 = null;
        Save_parts2 = null;
        Save_artifactSOs?.Clear();
        SceneManager.LoadScene("03_Menu");
    }
}
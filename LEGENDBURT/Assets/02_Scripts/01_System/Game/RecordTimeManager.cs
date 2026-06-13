using System;
using UnityEngine;

public class RecordTimeManager : MonoSingleton<RecordTimeManager>
{
    public float RecordTime { get; private set; } = 0;
    [SerializeField] private EventChannelSO playerChannel;
    private bool recording = false;
    private float startTime = 0;
    protected override void Awake()
    {
        base.Awake();
        playerChannel.AddListener<OnGameStartEvent>(HandleOnGameStartEvent);
        playerChannel.AddListener<OnGameOverEvent>(HandleOnGameOverEvent);
    }

    private void OnDestroy()
    {
        playerChannel.RemoveListener<OnGameStartEvent>(HandleOnGameStartEvent);
        playerChannel.RemoveListener<OnGameOverEvent>(HandleOnGameOverEvent);
    }

    private void HandleOnGameStartEvent(OnGameStartEvent @event)
    {
        recording = true;
        startTime = Time.time; // 시작 시점 스냅샷
    }

    private void HandleOnGameOverEvent(OnGameOverEvent @event)
    {
        recording = false;
        RecordTime = Time.time - startTime; // 경과 시간 계산
    }
}

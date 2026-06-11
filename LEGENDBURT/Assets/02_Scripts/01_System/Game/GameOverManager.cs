using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class GameOverManager : MonoSingleton<GameOverManager> // ±ÍÂú¾Æ¼­ ´ëÃæÇÑ ÄÚµå
{
    [SerializeField] private EventChannelSO playerChannel;
    public bool IsGameOver { get; private set; } = false;

    public int BestSpeed = 0;
    public int DriftCount = 0;
    public int EarnedItemCount = 0;
    public int FartCount = 0;


    protected override void Awake()
    {
        base.Awake();
        playerChannel.AddListener<OnGameOverRequestEvent>(HandleOnGameOverRequestEvent);
    }

    private void OnDestroy()
    {
        playerChannel.RemoveListener<OnGameOverRequestEvent>(HandleOnGameOverRequestEvent);
    }

    private void HandleOnGameOverRequestEvent(OnGameOverRequestEvent @event)
    {
        if (IsGameOver) return;

        playerChannel.RasiseEvent(PlayerEvents.OnGameOverEvent.Init(@event.IsGameSuccess));
        playerChannel.RasiseEvent(PlayerEvents.SetActivePlayerMovementInputEvent.Init(false));
        IsGameOver = true;
    }
}

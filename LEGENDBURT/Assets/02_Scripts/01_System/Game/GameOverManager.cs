    using DG.Tweening;
    using System;
    using TMPro;
    using UnityEngine;

    public class GameOverManager : MonoSingleton<GameOverManager> // 귀찮아서 대충한 코드
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

        private async void HandleOnGameOverRequestEvent(OnGameOverRequestEvent @event)
        {
            if (IsGameOver) return;


            playerChannel.RasiseEvent(PlayerEvents.OnGameOverEvent.Init(@event.IsGameSuccess));
            playerChannel.RasiseEvent(PlayerEvents.SetActivePlayerMovementInputEvent.Init(false));
            IsGameOver = true;

            await LeaderboardManager.Instance // 리더보드에 기록 갱신
            .SubmitTime(
            (StageType)(StageManager.Instance.CurrentStageIndex),
            RecordTimeManager.Instance.RecordTime);
        }
    }

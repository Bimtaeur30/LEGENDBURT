using Assets._02_Scripts._01_System.Stage;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI_Tutorial : MonoBehaviour
{

    [Header("Events")]
    [SerializeField] private EventChannelSO playerChannel;
    [SerializeField] private EventChannelSO stageChannel;
    [Header("Title")]
    [SerializeField] private TextMeshProUGUI OnSuccessTitleTxt;
    [SerializeField] private TextMeshProUGUI OnFailTitleTxt;
    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI timeTxt;
    [SerializeField] private TextMeshProUGUI bestSpeedTxt;
    [SerializeField] private TextMeshProUGUI driftCountTxt;
    [SerializeField] private TextMeshProUGUI earnedItemCountTxt;
    [SerializeField] private TextMeshProUGUI fartCountTxt;
    [Header("Button")]
    [SerializeField] private Button nextBtn;
    [SerializeField] private TextMeshProUGUI nextBtnTxt;
    [Header("Effect")]
    [SerializeField] private CanvasGroup[] hideCanvasGroup;
    [SerializeField] private CanvasGroup gameOverCanvasGroup;
    [SerializeField] private TopBottomBarLabel tbbl;
    [Header("Sound")]
    [SerializeField] private EventChannelSO SoundChannel;
    [SerializeField] private SoundClipSO FailSoundClip;
    [SerializeField] private SoundClipSO SuccessSoundClip;
    [SerializeField] private SoundClipSO TickClip;




    private void Awake()
    {
        playerChannel.AddListener<OnGameOverEvent>(HandleOnGameOverEvent);
        gameOverCanvasGroup.alpha = 0f;
        gameOverCanvasGroup.interactable = false;
        gameOverCanvasGroup.blocksRaycasts = false;

    }
    private void OnDestroy()
    {
        playerChannel.RemoveListener<OnGameOverEvent>(HandleOnGameOverEvent);
    }

    private void HandleOnGameOverEvent(OnGameOverEvent @event)
    {
        gameOverCanvasGroup.interactable = true;
        gameOverCanvasGroup.blocksRaycasts = true;

        OnSuccessTitleTxt.gameObject.SetActive(@event.IsGameSuccess);
        OnFailTitleTxt.gameObject.SetActive(!@event.IsGameSuccess);

        nextBtnTxt.text = @event.IsGameSuccess ? "로비로 돌아가기" : "튜토리얼 다시시작";
        if (@event.IsGameSuccess)
            SoundChannel.RasiseEvent(SoundEvents.PlaySoundEvent.Init(SuccessSoundClip));
        else
            SoundChannel.RasiseEvent(SoundEvents.PlaySoundEvent.Init(FailSoundClip));

        nextBtn.onClick.AddListener(() => HandleNextBtnClick(@event.IsGameSuccess));

        foreach (CanvasGroup cg in hideCanvasGroup)
        {
            cg.interactable = false;
            cg.DOFade(0f, 1f);
        }
        gameOverCanvasGroup.interactable = true;
        gameOverCanvasGroup.DOFade(1f, 1f);

        ShowStatsTxt();
        ShowRecordTimeTxt();
        tbbl.Show();
    }

    private void HandleNextBtnClick(bool isSuccess)
    {
        Debug.Log("HandleNextBtnClick 실행" + " isSuccess: " + isSuccess);

        if (isSuccess)
        {
            stageChannel.RasiseEvent(StageEvents.RemoveStageSaveDataEvent);
        }
        else
        {
            stageChannel.RasiseEvent(StageEvents.LoadTutorialEvent);
        }
    }

    private void HandleReciveData((PartsDataSO, PartsDataSO) tuple)
    {
        Debug.Log("HandleReciveData 실행");
        stageChannel.RasiseEvent(StageEvents.MoveNextStageEvent.Init(tuple.Item1, tuple.Item2, ArtifactManager.Instance.Equipped));
    }

    private void ShowStatsTxt()
    {
        bestSpeedTxt.text = "최대속도: " + GameOverManager.Instance.BestSpeed.ToString();
        fartCountTxt.text = "방귀 뀐 횟수: " + GameOverManager.Instance.FartCount.ToString();
        driftCountTxt.text = "드리프트 횟수: " + GameOverManager.Instance.DriftCount.ToString();
        earnedItemCountTxt.text = "획득한 유물 개수: " + GameOverManager.Instance.EarnedItemCount.ToString();
    }
    private void ShowRecordTimeTxt()
    {
        TimeSpan time = TimeSpan.FromSeconds(RecordTimeManager.Instance.RecordTime);
        string display = string.Format("{0:D2}:{1:D2}.{2:D2}",
            time.Minutes,
            time.Seconds,
            time.Milliseconds / 10); // 100ms 단위 → 2자리
        timeTxt.text = display;
    }
}

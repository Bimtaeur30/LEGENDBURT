using Assets._02_Scripts._01_System.Stage;
using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class TimeLineManager : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private EventChannelSO stageChannel;
    [SerializeField] private EventChannelSO playerChannel;
    [Header("Source")]
    [SerializeField] private PlayableDirector pd;
    [SerializeField] private CinemachineCamera timeLineCamera;
    [SerializeField] private CanvasGroup[] HideGroup;
    [SerializeField] private CanvasGroup[] ShowGroup;
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TopBottomBarLabel topBottomBarLabel;
    [SerializeField] private CheckStageSlot_UI checkSlot;
    [SerializeField] private RectTransform checkSlot_Parent;

    private bool timeLineRunning = true;
    Sequence seq;
    private void Awake()
    {
        stageChannel.AddListener<SetTimelineEvent>(HandleSetTimelineEvent);
        seq = DOTween.Sequence();
    }
    private void OnDestroy()
    {
        stageChannel.RemoveListener<SetTimelineEvent>(HandleSetTimelineEvent);
    }
    private void Start()
    {
        foreach(CanvasGroup group in HideGroup)
        {
            group.alpha = 0;
            group.interactable = false;
        }
        foreach(CanvasGroup group in ShowGroup)
        {
            group.alpha = 0;
            seq.Join(group.DOFade(1, 2));
        }
        titleTxt.text = StageManager.Instance.CurrentStageData.StageName;
        playerChannel.RasiseEvent(PlayerEvents.SetActivePlayerMovementInputEvent.Init(false));
        topBottomBarLabel.Show();

        if (StageManager.Instance.CurrentStageIndex >= 0)
            StartCoroutine(InstantiateCheckSlot());
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && timeLineRunning)
        {
            OnTimeLineEnd();
            pd.Stop();
            timeLineCamera.Priority = 0;
        }
    }
    public void OnTimeLineEnd()
    {
        seq.Kill();
        if (!timeLineRunning) return;
        timeLineRunning = false;

        Debug.Log("타임라인 끝남");
        foreach (CanvasGroup group in HideGroup)
        {
            group.DOFade(1, 1);
            group.interactable = true;
        }
        foreach (CanvasGroup group in ShowGroup)
            group.DOFade(0, 1);

        topBottomBarLabel.Hide();
    }

    private void HandleSetTimelineEvent(SetTimelineEvent @event)
    {
        pd.playableAsset = @event.timeline;
        pd.Play();
    }

    IEnumerator InstantiateCheckSlot()
    {
        yield return new WaitForSeconds(1f);

        int totalStage = StageManager.Instance.StageData.Length;
        int currentStage = StageManager.Instance.CurrentStageIndex;
        for (int i = 0; i < totalStage; i++)
        {
            CheckStageSlot_UI slot = Instantiate(checkSlot, checkSlot_Parent);
            bool highlight = i == currentStage;
            slot.Initialize(highlight, i + 1);

            yield return new WaitForSeconds(0.15f);
        }
    }
}

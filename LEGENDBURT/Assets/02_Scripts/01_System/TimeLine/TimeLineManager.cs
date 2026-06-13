using DG.Tweening;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class TimeLineManager : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private EventChannelSO playerChannel;
    [Header("Source")]
    [SerializeField] private PlayableDirector pd;
    [SerializeField] private CinemachineCamera timeLineCamera;
    [SerializeField] private CanvasGroup[] HideGroup;
    [SerializeField] private CanvasGroup[] ShowGroup;
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TopBottomBarLabel topBottomBarLabel;

    private bool timeLineRunning = true;
    Sequence seq;
    private void Awake()
    {
        seq = DOTween.Sequence();
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
}

using DG.Tweening;
using UnityEngine;

public class TimeLineManager : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private EventChannelSO playerChannel;
    [Header("Source")]
    [SerializeField] private CanvasGroup[] HideGroup;
    [SerializeField] private CanvasGroup[] ShowGroup;
    [SerializeField] private TopBottomBarLabel topBottomBarLabel;
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
            group.DOFade(1, 2);
        }
        playerChannel.RasiseEvent(PlayerEvents.SetActivePlayerMovementInputEvent.Init(false));
        topBottomBarLabel.Show();
    }
    public void OnTimeLineEnd()
    {
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

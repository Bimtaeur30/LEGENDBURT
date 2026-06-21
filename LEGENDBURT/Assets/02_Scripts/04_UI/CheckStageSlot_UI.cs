using DG.Tweening;
using TMPro;
using UnityEngine;

public class CheckStageSlot_UI : MonoBehaviour
{
    [SerializeField] private EventChannelSO soundChannel;
    [SerializeField] private SoundClipSO popClip;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CanvasGroup group;

    public void Initialize(bool isHighlight, int index)
    {
        group.alpha = isHighlight ? 1f : 0.3f;
        text.text = index.ToString();
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        soundChannel.RasiseEvent(SoundEvents.PlaySoundEvent.Init(popClip));
    }
}
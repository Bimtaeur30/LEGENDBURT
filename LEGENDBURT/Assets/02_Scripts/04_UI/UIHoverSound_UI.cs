using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverSound_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private EventChannelSO soundChannel;
    [SerializeField] private SoundClipSO clip_enter;
    [SerializeField] private SoundClipSO clip_exit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (clip_enter != null)
        {
            soundChannel.RasiseEvent(SoundEvents.PlaySoundEvent.Init(clip_enter));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (clip_exit != null)
        {
            soundChannel.RasiseEvent(SoundEvents.PlaySoundEvent.Init(clip_exit));
        }
    }
}

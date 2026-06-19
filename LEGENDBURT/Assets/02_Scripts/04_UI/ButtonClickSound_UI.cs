// ButtonClickSound_UI.cs
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickSound_UI : MonoBehaviour
{
    [SerializeField] private EventChannelSO soundChannel;
    [SerializeField] private SoundClipSO clip;
    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(HandleButtonClick);
    }

    private void HandleButtonClick()
    {
        // Trans 미전달 → spatialBlend = 0f (2D 재생)
        soundChannel.RasiseEvent(SoundEvents.PlaySoundEvent.Init(clip));
    }
}
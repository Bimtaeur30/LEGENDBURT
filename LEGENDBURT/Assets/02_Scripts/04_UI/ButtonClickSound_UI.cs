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
        soundChannel.RasiseEvent(SoundEvents.PlaySoundEvent.Init(clip));
    }
}

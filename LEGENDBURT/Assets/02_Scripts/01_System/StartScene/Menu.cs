using Assets._02_Scripts._01_System.Stage;
using UnityEngine;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    [SerializeField] private Nickname_UI nickname;
    [SerializeField] private EventChannelSO stageChannel;
    [SerializeField] private EventChannelSO soundChannel;
    [SerializeField] private SoundClipSO menuBGM;
    [SerializeField] private SoundClipSO fartClip;
    private bool moveing = false;

    private void Awake()
    {
        soundChannel.RasiseEvent(SoundEvents.PlaySoundEvent.Init(menuBGM));
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && nickname.IsNickNameChoosing == false && !moveing)
        {
            moveing = true;
            stageChannel.RasiseEvent(StageEvents.MoveNextStageEvent.Init(null, null, null));
            soundChannel.RasiseEvent(SoundEvents.PlaySoundEvent.Init(fartClip));

        }
    }
}

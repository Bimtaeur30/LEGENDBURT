using Assets._02_Scripts._01_System.Stage;
using UnityEngine;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    [SerializeField] private EventChannelSO stageChannel;
    [SerializeField] private EventChannelSO soundChannel;
    [SerializeField] private SoundClipSO menuBGM;

    private void Awake()
    {
        soundChannel.RasiseEvent(SoundEvents.PlaySoundEvent.Init(menuBGM));
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && NicknameManager.Instance.IsNickNameChoosing == false)
        {
            stageChannel.RasiseEvent(StageEvents.MoveNextStageEvent.Init(null, null, null));
        }
    }
}

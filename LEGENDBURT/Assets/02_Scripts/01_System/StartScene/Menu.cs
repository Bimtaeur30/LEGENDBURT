using Assets._02_Scripts._01_System.Stage;
using UnityEngine;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    [SerializeField] private EventChannelSO stageChannel;
    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            stageChannel.RasiseEvent(StageEvents.MoveNextStageEvent.Init(null, null, null));
        }
    }
}

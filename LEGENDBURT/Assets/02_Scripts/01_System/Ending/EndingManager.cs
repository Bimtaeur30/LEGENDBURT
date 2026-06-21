using Assets._02_Scripts._01_System.Stage;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private EventChannelSO stageChannel;

    public void QuitBtn()
    {
        stageChannel.RasiseEvent(StageEvents.RemoveStageSaveDataEvent);
    }
}

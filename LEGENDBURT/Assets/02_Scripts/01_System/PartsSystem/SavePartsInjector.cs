using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SavePartsInjector : MonoBehaviour
{
    [SerializeField] private EventChannelSO playerChannel;

    private void Start()
    {
        PartsDataSO part1 = StageManager.Instance.Save_parts1 == null ? null : StageManager.Instance.Save_parts1;
        PartsDataSO part2 = StageManager.Instance.Save_parts2 == null ? null : StageManager.Instance.Save_parts2;
        playerChannel.RasiseEvent(PlayerEvents.AttachPartsEvent.Init(part1, PartsJointPos.FirstSlot));
        playerChannel.RasiseEvent(PlayerEvents.AttachPartsEvent.Init(part2, PartsJointPos.SecondSlot));
    }
}

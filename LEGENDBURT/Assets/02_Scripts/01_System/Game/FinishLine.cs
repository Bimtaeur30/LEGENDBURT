using UnityEngine;

public class FinishLine : MonoBehaviour
{

    [SerializeField] private EventChannelSO playerChannel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerChannel.RasiseEvent(PlayerEvents.OnGameOverRequestEvent.Init(true));
        }
    }
}

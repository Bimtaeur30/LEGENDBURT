using UnityEngine;

public class ArtifactGiverBox : MonoBehaviour
{
    [SerializeField] private EventChannelSO playerChannel;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerChannel.RasiseEvent(PlayerEvents.OnItemSelectEvent);
            gameObject.SetActive(false);
        }
    }
}

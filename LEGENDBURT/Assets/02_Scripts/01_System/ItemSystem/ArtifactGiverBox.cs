using UnityEngine;

public class ArtifactGiverBox : MonoBehaviour
{
    [SerializeField] private EventChannelSO playerChannel;
    [SerializeField] private EventChannelSO soundChannel;
    [SerializeField] private SoundClipSO giveClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerChannel.RasiseEvent(PlayerEvents.OnItemSelectEvent);
            soundChannel.RasiseEvent(SoundEvents.PlaySoundEvent.Init(giveClip));

            gameObject.SetActive(false);
        }
    }
}

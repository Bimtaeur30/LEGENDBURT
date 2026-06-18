using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TutorialSender : MonoBehaviour
{
    [SerializeField] private TutorialData data;
    [SerializeField] private EventChannelSO tutorialChannel;
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialChannel.RasiseEvent(TutorialEvents.ShowTutorialPageEvent.Init(data));
        }
    }
}

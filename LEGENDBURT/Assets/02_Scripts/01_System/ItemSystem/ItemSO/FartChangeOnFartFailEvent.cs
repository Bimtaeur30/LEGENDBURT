using UnityEngine;

[CreateAssetMenu(menuName = "Artifact/Effect/FartChangeOnFartFail")]
public class FartChangeOnFartFailEvent : ArtifactEffectBase
{
    [Range(0f, 100f)] public float rate = 50;
    [SerializeField] private EventChannelSO playerChannel;

    public override void Apply(ArtifactContext ctx)
    {
        if (Random.Range(0, 100) >= rate)
        {
            Debug.Log("확률을 뚫고 방귀실패 찬스 발동");
            playerChannel.RasiseEvent(PlayerEvents.ActiveBurtEvent);
        }
    }
}

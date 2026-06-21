using UnityEngine;

[CreateAssetMenu(menuName = "Artifact/Effect/FartChangeOnFartFail")]
public class FartChangeOnFartFailEvent : ArtifactEffectBase
{
    [Range(0f, 100f)] public float rate = 50;
    [SerializeField] private EventChannelSO playerChannel;

    public override void Apply(ArtifactContext ctx)
    {
        if (Random.Range(0, 100) <= rate)
        {
            Debug.Log("È®·üÀ» ¶Õ°í ¹æ±Í½ÇÆÐ Âù½º ¹ßµ¿");
            playerChannel.RasiseEvent(PlayerEvents.ActiveBurtEvent);
        }
    }
}

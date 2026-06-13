using UnityEngine;

[CreateAssetMenu(menuName = "Artifact/Effect/StatBoost")]
public class StatBoostEffect : ArtifactEffectBase
{
    [Range(0f, 100f)] public float amountPercent; // 

    public override void Apply(ArtifactContext ctx)
        => ctx.player.MovementModule.MotorTorque += ctx.player.MovementModule.MotorTorque * (amountPercent / 100f);
}
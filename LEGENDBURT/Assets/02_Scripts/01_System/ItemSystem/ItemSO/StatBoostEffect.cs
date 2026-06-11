using UnityEngine;

[CreateAssetMenu(menuName = "Artifact/Effect/StatBoost")]
public class StatBoostEffect : ArtifactEffectBase
{
    public float amount;

    public override void Apply(ArtifactContext ctx)
        => ctx.player.MovementModule.MotorTorque += amount;
}
using UnityEngine;

public abstract class ArtifactEffectBase : ScriptableObject
{
    public TriggerType trigger;

    public abstract void Apply(ArtifactContext ctx);
}
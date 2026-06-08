using UnityEngine;

[CreateAssetMenu(menuName = "Artifact/ArtifactSO")]
public class ArtifactSO : ScriptableObject
{
    public string ArtifactName;
    public Sprite Icon;
    [TextArea]
    public string Description;
    public ArtifactEffectBase[] Effects;
}
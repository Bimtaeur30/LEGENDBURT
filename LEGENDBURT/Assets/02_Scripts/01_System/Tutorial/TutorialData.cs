using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "TutorialData", menuName = "Library/TutorialData")]
public class TutorialData : ScriptableObject
{
    [field:SerializeField] public string TutorialTitle { get; private set; }
    [TextArea][field:SerializeField] public string TutorialDescription { get; private set; }
    [field:SerializeField] public VideoClip TutorialVideo { get; private set; }
}

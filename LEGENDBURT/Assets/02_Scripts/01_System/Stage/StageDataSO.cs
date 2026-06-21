using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "StageDataSO", menuName = "Stage/StageDataSO")]
public class StageDataSO : ScriptableObject
{
    [field:SerializeField] public string StageName { get; private set; } = "¹«Á¦";
    [field: SerializeField] public int LimitedTime { get; private set; } = 30;
    [field: SerializeField] public Stage StagePrefab { get; private set; }
    [field: SerializeField] public TimelineAsset TimeLineAsset { get; private set; }
}

using UnityEngine;

[CreateAssetMenu(fileName = "StageDataSO", menuName = "Stage/StageDataSO")]
public class StageDataSO : ScriptableObject
{
    public string StageName = "¹«Á¦";
    public int LimitedTime = 30;
    public Stage StagePrefab;
}

using UnityEngine;

public class PartsDataSO : ScriptableObject
{
    public Sprite PartsIcon;
    public string PartsName;
    [TextArea]
    public string PartsDescription;
    public PartBase PartPrefab;
}

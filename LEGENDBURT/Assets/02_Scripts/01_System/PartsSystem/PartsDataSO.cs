using UnityEngine;
public class PartsDataSO : ScriptableObject
{
    [field:SerializeField] public string PartsName { get; private set; }
    [field: SerializeField] public PartsGrade PartsGrade { get; private set; }
    [field: SerializeField] public PartsGradeDataSO PartsGradeData { get; private set; }
    [field: SerializeField] public Sprite PartsIcon { get; private set; }
    [field: SerializeField] public PartBase PartPrefab { get; private set; }
    [field: SerializeField] public float CoolTime { get; private set; }
    [TextArea][field: SerializeField] public string PartsDescription { get; private set; }
}

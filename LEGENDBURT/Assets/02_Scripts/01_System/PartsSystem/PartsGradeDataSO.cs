using UnityEngine;

public enum PartsGrade
{
    Common, Rare, Legend
}

[CreateAssetMenu(fileName = "PartsGradeDataSO", menuName = "Library/PartsGradeDataSO")]
public class PartsGradeDataSO : ScriptableObject
{
    [System.Serializable]
    public class GradeData
    {
        public PartsGrade grade;
        public string displayName;
        public Color themeColor;
    }

    [SerializeField] private GradeData[] gradeDataList;

    public Color GetColor(PartsGrade grade)
    {
        foreach (var data in gradeDataList)
        {
            if (data.grade == grade)
                return data.themeColor;
        }
        Debug.LogWarning($"[PartsGradeDataSO] Grade not found: {grade}");
        return Color.white;
    }

    public string GetName(PartsGrade grade)
    {
        foreach (var data in gradeDataList)
        {
            if (data.grade == grade)
                return data.displayName;
        }
        Debug.LogWarning($"[PartsGradeDataSO] Grade not found: {grade}");
        return grade.ToString();
    }

#if UNITY_EDITOR
    [ContextMenu("Auto Fill Grade List")]
    private void AutoFillGradeList()
    {
        var grades = (PartsGrade[])System.Enum.GetValues(typeof(PartsGrade));
        gradeDataList = new GradeData[grades.Length];
        for (int i = 0; i < grades.Length; i++)
        {
            gradeDataList[i] = new GradeData
            {
                grade = grades[i],
                displayName = grades[i].ToString(),
                themeColor = Color.white
            };
        }
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}
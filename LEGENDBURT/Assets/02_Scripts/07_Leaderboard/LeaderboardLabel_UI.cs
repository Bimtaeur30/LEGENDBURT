using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardLabel_UI : MonoBehaviour
{
    [SerializeField] private GameObject highlightObj;

    [SerializeField] private TextMeshProUGUI nameLabelTxt;
    [SerializeField] private TextMeshProUGUI timeLabelTxt;
    [SerializeField] private TextMeshProUGUI gradeTxt;
    public void Initialize(int grade, string userName, float time, bool highlight)
    {
        gradeTxt.text = grade.ToString();
        nameLabelTxt.text = userName.ToString();
        timeLabelTxt.text = ShowRecordTimeTxt(time);

        highlightObj.SetActive(highlight);
    }

    private string ShowRecordTimeTxt(float timef)
    {
        TimeSpan time = TimeSpan.FromSeconds(timef);
        string display = string.Format("{0:D2}:{1:D2}.{2:D2}",
            time.Minutes,
            time.Seconds,
            time.Milliseconds / 10); // 100ms 단위 → 2자리
        return display;
    }
}

using TMPro;
using UnityEngine;

public class RPM_NumberSlot_Pref : MonoBehaviour
{
    [SerializeField] private GameObject onActiveObj;
    [SerializeField] private GameObject onHilightObj;
    [SerializeField] private TextMeshProUGUI numberTxt;

    public void Activate(string num, bool isAnchor)
    {
        onActiveObj.gameObject.SetActive(isAnchor);
        numberTxt.text = num;
    }

    public void OnHighlight(bool active)
    {
        onHilightObj.gameObject.SetActive(active);
    }
}

using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartsSlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Slider coolTimeSlider;
    [SerializeField] private Image SlotItemUI;
    private PartsDataSO currentData;
    private bool isCoolTime = false;
    public void SetSlot(PartsDataSO partsDataSO)
    {
        if (partsDataSO == null)
        {
            SlotItemUI.enabled = false;
            return;
        }
        SlotItemUI.enabled = true;
        SlotItemUI.sprite = partsDataSO.PartsIcon;
        currentData = partsDataSO;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentData == null) return;
        ToolTipManager.Instance.UseToolTip(currentData.PartsName, currentData.PartsDescription, ToolTipPivot.BottomRight);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipManager.Instance.StopUseToolTip();
    }

    public void ActiveSlot()
    {
        if (isCoolTime) return;
        if (currentData == null) return;
        StartCoroutine(CoolTimeCoroutine(currentData.CoolTime));
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.08f));
        seq.Append(transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f));
        seq.Append(transform.DOScale(Vector3.one, 0.07f));
    }

    IEnumerator CoolTimeCoroutine(float coolTime)
    {
        isCoolTime = true;
        float curTime = 0f;
        coolTimeSlider.value = 1f;

        while (coolTime > curTime)
        {
            curTime += Time.deltaTime;
            coolTimeSlider.value = Mathf.Lerp(1f, 0f, curTime / coolTime);
            yield return null;
        }

        coolTimeSlider.value = 0f; // 완료 보정
        isCoolTime = false;
    }
}

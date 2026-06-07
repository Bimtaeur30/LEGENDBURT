using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartsSlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image SlotItemUI;
    [SerializeField] private AnimationCurve ActiveAnimationCurve;
    private PartsDataSO currentData;
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
        if (currentData == null) return;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.08f));
        seq.Append(transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f));
        seq.Append(transform.DOScale(Vector3.one, 0.07f));
    }
}

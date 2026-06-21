using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemCard_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Action<ArtifactSO> OnItemCardSelectEvent;
    [SerializeField] private Image iconImage;
    private ArtifactSO m_data;
    private RectTransform rect;

    public void Initialize(ArtifactSO data)
    {
        m_data = data;
        iconImage.sprite = data.Icon;
        rect = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.DOSizeDelta(new Vector2(140, 140), 0.2f).SetUpdate(true);
        ToolTipManager.Instance.UseToolTip(m_data.ArtifactName, m_data.Description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.DOSizeDelta(new Vector2(130, 130), 0.2f).SetUpdate(true);
        ToolTipManager.Instance.StopUseToolTip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnItemCardSelectEvent?.Invoke(m_data);
    }
}

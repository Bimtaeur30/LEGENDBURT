using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClearBtn_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CanvasGroup cg;

    public void OnPointerEnter(PointerEventData eventData)
    {
        cg.DOFade(1f, 0.5f).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cg.DOFade(0f, 0.5f).SetUpdate(true);
    }
}

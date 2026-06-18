using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHoverEffect_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Rotation")]
    [SerializeField] private float hoverAngle = -10f;

    [Header("Animation")]
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private Ease ease = Ease.OutBack;

    private RectTransform rectTransform;
    private Quaternion defaultRotation;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        defaultRotation = rectTransform.localRotation;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.DOKill();

        rectTransform.DOLocalRotate(
            new Vector3(0f, 0f, hoverAngle),
            duration
        ).SetEase(ease).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DOKill();

        rectTransform.DOLocalRotate(
            defaultRotation.eulerAngles,
            duration
        ).SetEase(ease).SetUpdate(true);
    }
}
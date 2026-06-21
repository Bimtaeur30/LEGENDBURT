using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RectTransform))]
public class UITiltEffect_UI : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("Tilt")]
    [SerializeField] private float maxTiltAngle = 10f;

    [Header("Animation")]
    [SerializeField] private float rotationSpeed = 10f;

    private RectTransform rectTransform;
    private bool isHovering;

    private Quaternion targetRotation;
    private Quaternion initialRotation;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialRotation = rectTransform.localRotation;
        targetRotation = initialRotation;
    }

    private void Update()
    {
        if (isHovering)
        {
            UpdateTilt();
        }
        else
        {
            targetRotation = initialRotation;
        }

        rectTransform.localRotation = Quaternion.Slerp(
            rectTransform.localRotation,
            targetRotation,
            rotationSpeed * Time.unscaledDeltaTime);
    }

    private void UpdateTilt()
    {
        Vector2 localMousePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            Mouse.current.position.ReadValue(),
            null,
            out localMousePos);

        float halfWidth = rectTransform.rect.width * 0.5f;
        float halfHeight = rectTransform.rect.height * 0.5f;

        float normalizedX = Mathf.Clamp(localMousePos.x / halfWidth, -1f, 1f);
        float normalizedY = Mathf.Clamp(localMousePos.y / halfHeight, -1f, 1f);

        float xRotation = -normalizedY * maxTiltAngle;
        float yRotation = normalizedX * maxTiltAngle;

        targetRotation = initialRotation * Quaternion.Euler(xRotation, yRotation, 0f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}
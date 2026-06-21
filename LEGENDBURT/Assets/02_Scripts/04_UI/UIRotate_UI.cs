using UnityEngine;

public class UIRotate_UI : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 180f; // Degrees Per Second

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectTransform.Rotate(
            0f,
            0f,
            -rotationSpeed * Time.unscaledDeltaTime);
    }
}
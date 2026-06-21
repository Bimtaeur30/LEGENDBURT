using UnityEngine;
using UnityEngine.UI;

public class UVScroller : MonoBehaviour
{
    [SerializeField] private RawImage img;
    [SerializeField] private float x;
    [SerializeField] private float y;

    private void Update()
    {
        img.uvRect = new Rect(
            img.uvRect.position + new Vector2(x, y) * Time.unscaledDeltaTime,
            img.uvRect.size);
    }
}
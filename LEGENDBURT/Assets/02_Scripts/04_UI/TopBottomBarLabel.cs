using DG.Tweening;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TopBottomBarLabel : MonoBehaviour
{
    [SerializeField] private float BarHeight = 120;
    [SerializeField] private RectTransform TopBar;
    [SerializeField] private RectTransform BottomBar;

    public void Hide()
    {
        Move(0);
    }

    public void Show()
    {
        Move(BarHeight);
    }

    private void Move(float target)
    {
        TopBar.DOSizeDelta(
    new Vector2(TopBar.sizeDelta.x, target),
    0.5f
);
        BottomBar.DOSizeDelta(
            new Vector2(BottomBar.sizeDelta.x, target),
            0.5f
        );
    }
}

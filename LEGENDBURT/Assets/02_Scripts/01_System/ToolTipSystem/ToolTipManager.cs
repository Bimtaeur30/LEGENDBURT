using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum ToolTipPivot
{
    Center, Right, Top, Bottom, Left,
    TopRight, BottomRight, TopLeft, BottomLeft
}

public class ToolTipManager : MonoSingleton<ToolTipManager>
{
    [SerializeField] private RectTransform ToolTipPanel;
    [SerializeField] private TextMeshProUGUI TitleTxt;
    [SerializeField] private TextMeshProUGUI DescriptionTxt;

    private bool isUsing;
    private ToolTipPivot currentPivot;

    public void UseToolTip(string title, string description, ToolTipPivot pivot = ToolTipPivot.Center)
    {
        TitleTxt.text = title;
        DescriptionTxt.text = description;

        currentPivot = pivot;
        isUsing = true;

        ToolTipPanel.gameObject.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(ToolTipPanel);

        UpdateToolTipPosition();
    }

    public void StopUseToolTip()
    {
        isUsing = false;
        ToolTipPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isUsing) return;

        UpdateToolTipPosition();
    }

    private void UpdateToolTipPosition()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        Vector2 size = ToolTipPanel.rect.size;
        Vector2 offset = GetOffsetByPivot(currentPivot, size);

        ToolTipPanel.position = mousePos + offset;
    }

    private Vector2 GetOffsetByPivot(ToolTipPivot pivot, Vector2 size)
    {
        return pivot switch
        {
            ToolTipPivot.Center => Vector2.zero,

            ToolTipPivot.Right => new Vector2(-size.x * 0.5f, 0f),
            ToolTipPivot.Left => new Vector2(size.x * 0.5f, 0f),
            ToolTipPivot.Top => new Vector2(0f, -size.y * 0.5f),
            ToolTipPivot.Bottom => new Vector2(0f, size.y * 0.5f),

            ToolTipPivot.TopRight => new Vector2(-size.x * 0.5f, -size.y * 0.5f),
            ToolTipPivot.TopLeft => new Vector2(size.x * 0.5f, -size.y * 0.5f),
            ToolTipPivot.BottomRight => new Vector2(-size.x * 0.5f, size.y * 0.5f),
            ToolTipPivot.BottomLeft => new Vector2(size.x * 0.5f, size.y * 0.5f),

            _ => Vector2.zero
        };
    }
}
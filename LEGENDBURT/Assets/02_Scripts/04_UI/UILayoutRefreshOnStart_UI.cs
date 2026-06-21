using UnityEngine;
using UnityEngine.UI;

public class UILayoutRefreshOnStart_UI : MonoBehaviour
{
    private void Start()
    {
        Refresh();
    }

    [ContextMenu("Refresh Layout")]
    public void Refresh()
    {
        Canvas.ForceUpdateCanvases();

        RectTransform root = transform as RectTransform;

        if (root == null)
            return;

        RefreshRecursive(root);

        Canvas.ForceUpdateCanvases();
    }

    private void RefreshRecursive(RectTransform current)
    {
        for (int i = 0; i < current.childCount; i++)
        {
            if (current.GetChild(i) is RectTransform child)
            {
                RefreshRecursive(child);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(current);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image ItemIcon;
    private ArtifactSO currentData;
    public void Initialize(ArtifactSO artifactSO)
    {
        ItemIcon.sprite = artifactSO.Icon;
        currentData = artifactSO;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentData == null) return;
        ToolTipManager.Instance.UseToolTip(currentData.ArtifactName, currentData.Description, ToolTipPivot.TopLeft);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipManager.Instance.StopUseToolTip();
    }
}

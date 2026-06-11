using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PartsEquip_UI : MonoBehaviour
{
    // 墨靛 积己窍绊 瘤快绰芭 救沁澜.
    // ParsCardSelector_UI俊辑 捞率栏肺 ActivateEquipParts 秦拎具窃.
    [Header("Events")]
    [SerializeField] private EventChannelSO playerChannel;
    [Header("UI")]
    [SerializeField] private CanvasGroup[] hideGroups;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup equipBtnGroup;
    [SerializeField] private Image partsIcon;
    [SerializeField] private Button equipBtn_L;
    [SerializeField] private Button equipBtn_R;
    private bool onEquipEvent = false;
    private PartsDataSO myData;

    private void Awake()
    {
        equipBtn_L.onClick.AddListener(() => HandleEquipBtnPressed(PartsJointPos.FirstSlot));
        equipBtn_R.onClick.AddListener(() => HandleEquipBtnPressed(PartsJointPos.SecondSlot));
    }

    private void HandleEquipBtnPressed(PartsJointPos pos)
    {
        Debug.Log(playerChannel);
        Debug.Log(myData);
        Debug.Log(myData?.PartPrefab);
        // 捞力 咯扁辑 何馒秦林搁 等促.
        DeactivateEquipParts();
        playerChannel.RasiseEvent(PlayerEvents.AttachPartsEvent.Init(myData.PartPrefab, pos));
    }

    public void ActivateEquipParts(PartsDataSO data)
    {
        Debug.Log("蛔蛔蛔蛔蛔蛔蛔蛔蛔蛔蛔蛔蛔蛔蛔蛔蛔蛔蛔蛔蛔蛔蛔蛔");
        myData = data;
        Debug.Assert(data != null, "圈费费费费费费费费费费费费费费费费费费费费费费费费");
        Debug.Assert(data.PartPrefab != null, "圈费费费费费费费费费费费费费费费费费费费费费费费费222222222222");
        onEquipEvent = true;
        partsIcon.sprite = data.PartsIcon;

        canvasGroup.DOFade(1f, 1f);
        equipBtnGroup.DOFade(1f, 1f);
        equipBtnGroup.interactable = true;

        foreach (CanvasGroup c in hideGroups)
            c.DOFade(0f, 0.5f);
    }

    public void DeactivateEquipParts()
    {
        
        canvasGroup.DOFade(0f, 0.5f);
        equipBtnGroup.DOFade(0f, 0.5f);
        equipBtnGroup.interactable = false;

        foreach (CanvasGroup c in hideGroups)
            c.DOFade(1f, 0.5f);
    }

    private void Update()
    {
        if (onEquipEvent)
        {
            partsIcon.rectTransform.position = Mouse.current.position.ReadValue();
        }
    }
}

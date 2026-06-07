using System;
using UnityEngine;
using UnityEngine.UI;

public class PartsInventory_UI : MonoBehaviour
{
    [SerializeField] private EventChannelSO PlayerChannel;
    [SerializeField] private PartsSlot_UI SlotItemUI_01;
    [SerializeField] private PartsSlot_UI SlotItemUI_02;

    private void Awake()
    {
        PlayerChannel.AddListener<AttachPartsEvent>(HandleAttachPartsEvent);
        PlayerChannel.AddListener<RemovePartsEvent>(HandleRemovePartsEvent);
        PlayerChannel.AddListener<ActivePartsEvent>(HandleActivePartsEvent);

    }
    private void HandleActivePartsEvent(ActivePartsEvent @event)
    {
        if (@event.JointPos == PartsJointPos.FirstSlot)
        {
            SlotItemUI_01.ActiveSlot();
        }
        else if (@event.JointPos == PartsJointPos.SecondSlot)
        {
            SlotItemUI_02.ActiveSlot();
        }
    }

    private void HandleRemovePartsEvent(RemovePartsEvent @event)
    {
        switch (@event.JointPos)
        {
            case PartsJointPos.FirstSlot:
                // 이미지에 아이템 이미지 적용시키기.
                //SlotItemUI_01.enabled = false;
                SlotItemUI_01.SetSlot(null);
                break;
            case PartsJointPos.SecondSlot:
                // 이미지에 아이템 이미지 적용시키기.
                //SlotItemUI_02.enabled = false;
                SlotItemUI_02.SetSlot(null);
                break;
        }
    }

    private void HandleAttachPartsEvent(AttachPartsEvent @event)
    {
        switch(@event.JointPos)
        {
            case PartsJointPos.FirstSlot:
                // 이미지에 아이템 이미지 적용시키기.
                SlotItemUI_01.enabled = true;
                SlotItemUI_01.SetSlot(@event.Parts.PartsDataSO);

                break;
            case PartsJointPos.SecondSlot:
                // 이미지에 아이템 이미지 적용시키기.
                SlotItemUI_02.enabled = true;
                SlotItemUI_02.SetSlot(@event.Parts.PartsDataSO);
                break;
        }
    }
}

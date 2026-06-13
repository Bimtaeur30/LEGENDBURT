using Assets._02_Scripts._01_System.Stage;
using System;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public enum PartsJointPos
{
    FirstSlot = 0, SecondSlot = 1
}

public class PartsModule : MonoBehaviour, IModule, IAfterInitModule
{
    [SerializeField] private EventChannelSO playerChannel;
    [SerializeField] private EventChannelSO stageChannel;
    [SerializeField] private Transform PartsJoint_01;
    [SerializeField] private Transform PartsJoint_02;

    private Player player;
    public IParts CurrentFirstParts { get; private set; } = null;
    public IParts CurrentSecondParts { get; private set; } = null;
    public void Initialize(ModuleOwner owner)
    {
        player = owner as Player;
    }
    public void AfterInitalize()
    {
        playerChannel.AddListener<AttachPartsEvent>(HandleAttachPartsEvent);
        playerChannel.AddListener<RemovePartsEvent>(HandleRemovePartsEvent);
        playerChannel.AddListener<ActivePartsEvent>(HandleActivePartsEvent);

        stageChannel.AddListener<GetEquipedPartsDataEvent>(HandleGetEquipedPartsDataEvent);
    }
    private void OnDestroy()
    {
        playerChannel.RemoveListener<AttachPartsEvent>(HandleAttachPartsEvent);
        playerChannel.RemoveListener<RemovePartsEvent>(HandleRemovePartsEvent);
        playerChannel.RemoveListener<ActivePartsEvent>(HandleActivePartsEvent);

        stageChannel.RemoveListener<GetEquipedPartsDataEvent>(HandleGetEquipedPartsDataEvent);
    }
    private void HandleGetEquipedPartsDataEvent(GetEquipedPartsDataEvent @event)
    {
        PartsDataSO data1 = (CurrentFirstParts == null ? null : CurrentFirstParts.PartsDataSO);
        PartsDataSO data2 = (CurrentSecondParts == null ? null : CurrentSecondParts.PartsDataSO);
        @event.ReciveAction?.Invoke((data1, data2));
    }

    private void HandleActivePartsEvent(ActivePartsEvent @event)
    {
        if (@event.JointPos == PartsJointPos.FirstSlot && CurrentFirstParts != null)
        {
            CurrentFirstParts.Activate();
            player.ChatModule.GenerateChat(CurrentFirstParts.PartsDataSO.PartsName + " 발동!");
        }
        else if (@event.JointPos == PartsJointPos.SecondSlot && CurrentSecondParts != null)
        {
            CurrentSecondParts.Activate();
            player.ChatModule.GenerateChat(CurrentSecondParts.PartsDataSO.PartsName + " 발동!");
        }
    }
    private void HandleRemovePartsEvent(RemovePartsEvent @event)
    {
        Transform joint = GetJointTransform(@event.JointPos);
        DestroyChilds(joint);

        switch(@event.JointPos)
        {
            case PartsJointPos.FirstSlot:
                CurrentFirstParts = null;
                break;
            case PartsJointPos.SecondSlot:
                CurrentSecondParts = null;
                break;
        }
    }
    private void HandleAttachPartsEvent(AttachPartsEvent @event)
    {
        if (@event.Parts == null) return;

        Transform joint = GetJointTransform(@event.JointPos);

        DestroyChilds(joint);

        PartBase instance = InstantiateToJoint(@event.Parts.PartPrefab, joint);

        switch (@event.JointPos)
        {
            case PartsJointPos.FirstSlot:
                CurrentFirstParts = instance;
                break;

            case PartsJointPos.SecondSlot:
                CurrentSecondParts = instance;
                break;
        }

        //playerChannel.RasiseEvent(PlayerEvents.OnGameStartEvent);
        //playerChannel.RasiseEvent(PlayerEvents.SetActivePlayerMovementInputEvent.Init(true));
    }
    private Transform GetJointTransform(PartsJointPos jointPos)
    {
        Transform joint = PartsJoint_01.transform;
        switch (jointPos)
        {
            case PartsJointPos.FirstSlot:
                joint = PartsJoint_01.transform;
                break;
            case PartsJointPos.SecondSlot:
                joint = PartsJoint_02.transform;
                break;
        }

        return joint;
    }
    private void DestroyChilds(Transform joint) // 기존 파츠 삭제
    {
        for (int i = 0; i < joint.childCount; i++)
        {
            GameObject childObject = joint.GetChild(i).gameObject;
            if (childObject.TryGetComponent<PartBase>(out PartBase part))
            {
                part.DestroyParts();
                Destroy(childObject);
            }
        }
    }
    private PartBase InstantiateToJoint(PartBase part, Transform joint)
    {
        PartBase parts = Instantiate(part, joint);

        parts.Initialize(player);
        parts.transform.localScale = Vector3.one;
        parts.transform.localRotation = Quaternion.identity;
        parts.transform.localPosition = Vector3.zero;

        return parts;
    }
}
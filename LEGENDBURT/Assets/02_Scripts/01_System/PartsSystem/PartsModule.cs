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
    [SerializeField] private Transform PartsJoint_01;
    [SerializeField] private Transform PartsJoint_02;

    private Player player;
    private IParts currentFirstParts;
    private IParts currentSecondParts;
    public void Initialize(ModuleOwner owner)
    {
        player = owner as Player;
    }
    public void AfterInitalize()
    {
        playerChannel.AddListener<AttachPartsEvent>(HandleAttachPartsEvent);
        playerChannel.AddListener<RemovePartsEvent>(HandleRemovePartsEvent);
        playerChannel.AddListener<ActivePartsEvent>(HandleActivePartsEvent);
    }

    private void HandleActivePartsEvent(ActivePartsEvent @event)
    {
        if (@event.JointPos == PartsJointPos.FirstSlot && currentFirstParts != null)
        {
            currentFirstParts.Activate();
        }
        else if (@event.JointPos == PartsJointPos.SecondSlot && currentSecondParts != null)
        {
            currentSecondParts.Activate();
        }
    }
    private void HandleRemovePartsEvent(RemovePartsEvent @event)
    {
        Transform joint = GetJointTransform(@event.JointPos);
        DestroyChilds(joint);

        switch(@event.JointPos)
        {
            case PartsJointPos.FirstSlot:
                currentFirstParts = null;
                break;
            case PartsJointPos.SecondSlot:
                currentSecondParts = null;
                break;
        }
    }
    private void HandleAttachPartsEvent(AttachPartsEvent @event)
    {
        Transform joint = GetJointTransform(@event.JointPos);

        DestroyChilds(joint);

        PartBase instance = InstantiateToJoint(@event.Parts, joint);

        switch (@event.JointPos)
        {
            case PartsJointPos.FirstSlot:
                currentFirstParts = instance;
                break;

            case PartsJointPos.SecondSlot:
                currentSecondParts = instance;
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
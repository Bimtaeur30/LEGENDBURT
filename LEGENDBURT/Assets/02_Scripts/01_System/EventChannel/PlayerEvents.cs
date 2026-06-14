using UnityEngine;

public static class PlayerEvents
{
    public static readonly OnGameStartEvent OnGameStartEvent = new OnGameStartEvent(); // 신호등 꺼질 시
    public static readonly OnGameReadyEvent OnGameReadyEvent = new OnGameReadyEvent(); // 시작 파츠 부착 시
    public static readonly OnGameOverEvent OnGameOverEvent = new OnGameOverEvent(); // 끝지점 통과 / 실패 시
    public static readonly OnGameOverRequestEvent OnGameOverRequestEvent = new OnGameOverRequestEvent();

    public static readonly ActiveBurtEvent ActiveBurtEvent = new ActiveBurtEvent();

    public static readonly AttachPartsEvent AttachPartsEvent = new AttachPartsEvent();
    public static readonly RemovePartsEvent RemovePartsEvent = new RemovePartsEvent();
    public static readonly ActivePartsEvent ActivePartsEvent = new ActivePartsEvent();

    public static readonly OnCardSelectEvent OnCardSelectEvent = new OnCardSelectEvent();
    public static readonly EquipItemEvent EquipItemEvent = new EquipItemEvent();
    public static readonly SetActivePlayerMovementInputEvent SetActivePlayerMovementInputEvent = new SetActivePlayerMovementInputEvent();

    public static readonly OnItemSelectEvent OnItemSelectEvent = new OnItemSelectEvent();
}

public class OnGameStartEvent : GameEvent { }
public class OnGameReadyEvent : GameEvent { }
public class OnGameOverEvent : GameEvent
{
    public bool IsGameSuccess { get; private set; } // 스테이지 성공/ 실패 여부
    public OnGameOverEvent Init(bool isOver)
    {
        IsGameSuccess = isOver;
        return this;
    }
}
public class OnGameOverRequestEvent : GameEvent
{
    public bool IsGameSuccess { get; private set; } // 스테이지 성공/ 실패 여부
    public OnGameOverRequestEvent Init(bool isOver)
    {
        IsGameSuccess = isOver;
        return this;
    }
}
public class SetActivePlayerMovementInputEvent : GameEvent
{
    public bool IsActive { get; private set; } // 스테이지 성공/ 실패 여부
    public SetActivePlayerMovementInputEvent Init(bool isActive)
    {
        IsActive = isActive;
        return this;
    }
}
public class ActiveBurtEvent : GameEvent { }
public class AttachPartsEvent : GameEvent
{
    public PartsDataSO Parts;
    public PartsJointPos JointPos;
    public  AttachPartsEvent Init(PartsDataSO parts, PartsJointPos jointPos)
    {
        this.Parts = parts;
        this.JointPos = jointPos;
        return this;
    }
}
public class RemovePartsEvent : GameEvent
{
    public PartsJointPos JointPos;
    public RemovePartsEvent Init(PartsJointPos jointPos)
    {
        this.JointPos = jointPos;
        return this;
    }
}
public class ActivePartsEvent : GameEvent
{
    public PartsJointPos JointPos;
    public ActivePartsEvent Init(PartsJointPos jointPos)
    {
        this.JointPos = jointPos;
        return this;
    }
}
public class OnCardSelectEvent : GameEvent { }
public class EquipItemEvent : GameEvent
{
    public ArtifactSO artifactSO;
    public EquipItemEvent Init(ArtifactSO artifactSO)
    {
        this.artifactSO = artifactSO;
        return this;
    }
}
public class OnItemSelectEvent : GameEvent { }


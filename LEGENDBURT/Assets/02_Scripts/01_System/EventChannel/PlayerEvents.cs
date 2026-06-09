using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class PlayerEvents
{
    public static readonly OnGameStartEvent OnGameStartEvent = new OnGameStartEvent();
    public static readonly OnGameOverEvent OnGameOverEvent = new OnGameOverEvent();
    public static readonly ActiveBurtEvent ActiveBurtEvent = new ActiveBurtEvent();
    public static readonly AttachPartsEvent AttachPartsEvent = new AttachPartsEvent();
    public static readonly RemovePartsEvent RemovePartsEvent = new RemovePartsEvent();
    public static readonly ActivePartsEvent ActivePartsEvent = new ActivePartsEvent();
    public static readonly OnCardSelectEvent OnCardSelectEvent = new OnCardSelectEvent();
    public static readonly EquipItemEvent EquipItemEvent = new EquipItemEvent();

}

public class OnGameStartEvent : GameEvent { }
public class OnGameOverEvent : GameEvent { }
public class ActiveBurtEvent : GameEvent { }
public class AttachPartsEvent : GameEvent
{
    public PartBase Parts;
    public PartsJointPos JointPos;
    public  AttachPartsEvent Init(PartBase parts, PartsJointPos jointPos)
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

using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class PlayerEvents
{
    public static readonly ActiveBurtEvent ActiveBurtEvent = new ActiveBurtEvent();
    public static readonly AttachPartsEvent AttachPartsEvent = new AttachPartsEvent();
    public static readonly RemovePartsEvent RemovePartsEvent = new RemovePartsEvent();
    public static readonly ActivePartsEvent ActivePartsEvent = new ActivePartsEvent();
    public static readonly OnCardSelectEvent OnCardSelectEvent = new OnCardSelectEvent();

}

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

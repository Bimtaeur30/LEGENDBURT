using UnityEngine;

public static class PlayerEvents
{
    public static readonly ActiveBurtEvent ActiveBurtEvent = new ActiveBurtEvent();
}

public class ActiveBurtEvent : GameEvent { }

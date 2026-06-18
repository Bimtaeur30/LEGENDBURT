using Assets._02_Scripts._01_System.Stage;
using UnityEngine;

public static class TutorialEvents
{
    public static ShowTutorialPageEvent ShowTutorialPageEvent = new ShowTutorialPageEvent();
}

public class ShowTutorialPageEvent : GameEvent
{
    public TutorialData TutorialData { get; private set; }
    public ShowTutorialPageEvent Init(TutorialData data)
    {
        TutorialData = data;
        return this;
    }
}

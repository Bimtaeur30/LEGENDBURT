using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Timeline;

namespace Assets._02_Scripts._01_System.Stage
{
    public static class StageEvents
    {
        public static MoveNextStageEvent MoveNextStageEvent = new MoveNextStageEvent();
        public static LoadTutorialEvent LoadTutorialEvent = new LoadTutorialEvent();
        public static CreateStageSaveDataEvent CreateStageSaveDataEvent = new CreateStageSaveDataEvent();
        public static RemoveStageSaveDataEvent RemoveStageSaveDataEvent = new RemoveStageSaveDataEvent();

        public static GetEquipedPartsDataEvent GetEquipedPartsDataEvent = new GetEquipedPartsDataEvent();
        public static SetTimelineEvent SetTimelineEvent = new SetTimelineEvent();
    }

    public class MoveNextStageEvent : GameEvent // 스테이지 이동
    {
        public PartsDataSO FirstParts;
        public PartsDataSO SecondParts;
        public List<ArtifactSO> ArtifactLIst;

        public MoveNextStageEvent Init(PartsDataSO firstParts, PartsDataSO secondParts, List<ArtifactSO> artifactLIst)
        {
            FirstParts = firstParts;
            SecondParts = secondParts;
            ArtifactLIst = artifactLIst;
            return this;
        }
    }

    public class LoadTutorialEvent : GameEvent { } // 튜토리얼 시작
    public class CreateStageSaveDataEvent : GameEvent { } // 스테이지 시작
    public class RemoveStageSaveDataEvent : GameEvent { } // 게임 오버
    public class SetTimelineEvent : GameEvent
    {
        public TimelineAsset timeline;
        public SetTimelineEvent Init(TimelineAsset asset)
        {
            timeline = asset;
            return this;
        }
    }
    public class GetEquipedPartsDataEvent : GameEvent //  파츠 받기
    {
        public Action<(PartsDataSO, PartsDataSO)> ReciveAction;
        public GetEquipedPartsDataEvent Init(Action<(PartsDataSO, PartsDataSO)> reciveAction)
        {
            ReciveAction = reciveAction;
            return this;
        }
    }
}

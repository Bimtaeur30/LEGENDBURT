//using UnityEngine;


//// ── 방귀 파워 배율 ────────────────────────────────────────────────────────────

//[CreateAssetMenu(menuName = "Artifact/Effect/FartPower")]
//public class FartPowerEffect : ArtifactEffectBase
//{
//    [Range(0f, 100f)] public float rate = 50;
//    public float multiplier = 1.3f;

//    public override void Apply(ArtifactContext ctx)
//    {
//        var fart = ctx.GetModule<BoostModule>();
//        Debug.Assert(fart != null, "부스트모듈을 가져오지 못했습니다");

//        if (fart != null)
//        {
//            if (Random.Range(0, 100) >= rate)
//            {
//                fart.BoostMultiplier *= multiplier;
//                Debug.Log(rate + "확률을 뚫고 방구 부스트 활성");
//            }
//            else
//            {
//                Debug.Log(rate + "확률 실패");
//            }
//        }
//    }
//}

//// ── 장착 시 스탯 증가 ─────────────────────────────────────────────────────────

//[CreateAssetMenu(menuName = "Artifact/Effect/StatBoost")]
//public class StatBoostEffect : ArtifactEffectBase
//{
//    public float amount;

//    public override void Apply(ArtifactContext ctx)
//        => ctx.player.MovementModule.MotorTorque += amount;
//}
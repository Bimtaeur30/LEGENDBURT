using System.Collections;
using UnityEngine;

public class Parts_TestBoost : PartBase
{
    private ParticleSystem particle;
    private BoosterPartsDataSO boosterPartsDataSO;

    private BoostModule boostModule;

    public override void Initialize(ModuleOwner owner)
    {
        base.Initialize(owner);

        // 초기화
        boosterPartsDataSO = PartsDataSO as BoosterPartsDataSO;
        Debug.Assert(boosterPartsDataSO != null, "부스트 파츠의 파츠 데이터SO는 BoosterPartsDataSO 형식이어야 합니다.");
        boostModule = owner.GetModule<BoostModule>();
        Debug.Assert(boostModule != null, "오너에 부스트 모듈이 부착되어있지 않습니다.");

        // 파티클 세팅
        particle = Instantiate(boosterPartsDataSO.BoostParticlePrefab, player.transform);
        particle.gameObject.transform.localScale = Vector3.one;
        particle.gameObject.transform.localPosition = Vector3.zero;
        particle.Stop();
    }

    public override void Activate()
    {
        Debug.Log($"name = {gameObject.name}");
        Debug.Log($"activeSelf = {gameObject.activeSelf}");
        Debug.Log($"activeInHierarchy = {gameObject.activeInHierarchy}");
        Debug.Log($"enabled = {enabled}");

        StartCoroutine(BoostCoroutine());
        particle.Play();
    }

    IEnumerator BoostCoroutine()
    {
        float duration = 3f; // 테스트 값
        float interval = 0.5f; // 테스트 값 (이거랑 위에서SO로 빼둬야함.)
        float elapsed = 0f;

        while (elapsed < duration)
        {
            boostModule.Active_Boost(boosterPartsDataSO.BoostForce);

            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }

        Deactivate();
    }

    public override void Deactivate()
    {
        particle.Stop();
    }

    public override void DestroyParts()
    {
        particle.Stop();
        Destroy(particle.gameObject);
    }

}

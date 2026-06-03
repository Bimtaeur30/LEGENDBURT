using DG.Tweening;
using System;
using System.Net.NetworkInformation;
using Unity.Cinemachine;
using UnityEngine;

public class BoostModule : MonoBehaviour, IModule, IAfterInitModule
{
    [SerializeField] private EventChannelSO PlayerChannel;

    [SerializeField] private float boostForce = 8000f;
    [SerializeField] private ParticleSystem burtParticle;
    [SerializeField] private GameObject HipModel;

    private CinemachineImpulseSource impulseSource;
    private Player player;

    public void Initialize(ModuleOwner owner)
    {
        player = owner as Player;
        impulseSource = GetComponent<CinemachineImpulseSource>();
        Debug.Assert(impulseSource != null, "부스트 모듈에 시네머신 임펄스 소스가 없습니다.");
    }
    public void AfterInitalize()
    {
        PlayerChannel.AddListener<ActiveBurtEvent>(HandleActiveBurtEvent);
    }

    private void HandleActiveBurtEvent(ActiveBurtEvent @event)
    {
        Activate();
    }

    public void Activate()
    {
        player.Rigid.AddForce(transform.forward * boostForce, ForceMode.Acceleration);
        impulseSource.GenerateImpulse();
        burtParticle.Play();
        HipModel.transform.DOScale(1.2f, 0.2f).OnComplete(() => {
            HipModel.transform.DOScale(1f, 0.1f);
        });
    }

}
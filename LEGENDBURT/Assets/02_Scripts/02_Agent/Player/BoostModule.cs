using DG.Tweening;
using System;
using System.Net.NetworkInformation;
using Unity.Cinemachine;
using UnityEngine;

public class BoostModule : MonoBehaviour, IModule, IAfterInitModule
{
    [SerializeField] private EventChannelSO PlayerChannel;

    [SerializeField] private float boostForce = 200f;
    [SerializeField] private ParticleSystem burtParticle;
    [SerializeField] private GameObject HipModel;

    public float BoostMultiplier = 1f;
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
        Activate_Burt();
    }

    public void Activate_Burt()
    {
        ArtifactContext at = new ArtifactContext();
        at.player = player;
        at.SetModule(this);
        ArtifactManager.Instance.Fire(TriggerType.OnBurtSuccess, at);

        Active_Boost(boostForce * BoostMultiplier);
        impulseSource.GenerateImpulse();

        burtParticle.Play();
        HipModel.transform.DOScale(1.2f, 0.2f).OnComplete(() => {
            HipModel.transform.DOScale(1f, 0.1f);
        });

        GameOverManager.Instance.FartCount++;
    }

    public void Active_Boost(float force)
    {
        player.Rigid.AddForce(transform.forward * force, ForceMode.Acceleration);
    }
}
using System;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class ArtifactManager : MonoSingleton<ArtifactManager>
{
    [Header("Events")]
    [SerializeField] private EventChannelSO playerChannel;
    [Header("Player")]
    [SerializeField] private Player _playerStats;

    private readonly List<ArtifactSO> _equipped = new();
    private readonly Dictionary<ArtifactEffectBase, float> _timers = new();

    // ── 장착 / 해제 ───────────────────────────────────────────────────────────
    protected override void Awake()
    {
        base.Awake();
        playerChannel.AddListener<EquipItemEvent>(HandleEquipItemEvent);
    }

    private void HandleEquipItemEvent(EquipItemEvent @event)
    {
        _equipped.Add(@event.artifactSO);
        GameOverManager.Instance.EarnedItemCount++;
        Fire(TriggerType.OnEquip, new ArtifactContext { player = _playerStats });
    }

    public void Unequip(ArtifactSO artifact)
    {
        Fire(TriggerType.OnUnequip, new ArtifactContext { player = _playerStats });
        _equipped.Remove(artifact);

        foreach (var effect in artifact.Effects)
            _timers.Remove(effect);
    }

    // ── 트리거 발동 (외부 시스템에서 호출) ────────────────────────────────────

    public ArtifactContext Fire(TriggerType trigger, ArtifactContext ctx)
    {
        foreach (var artifact in _equipped)
            foreach (var effect in artifact.Effects)
                if (effect.trigger == trigger)
                    effect.Apply(ctx);
        return ctx;
    }

    // ── 주기 트리거 (EveryNSeconds) ────────────────────────────────────────────

    //private void Update()
    //{
    //    var ctx = new ArtifactContext { player = _playerStats };

    //    foreach (var artifact in _equipped)
    //        foreach (var effect in artifact.effects)
    //        {
    //            if (effect.trigger != TriggerType.EveryNSeconds) continue;
    //            if (effect is not PeriodicEffect periodic) continue;

    //            _timers.TryGetValue(effect, out float t);
    //            t += Time.deltaTime;

    //            if (t >= periodic.interval)
    //            {
    //                t -= periodic.interval;   // 잔여 시간 유지 (누적 방지)
    //                effect.Apply(ctx);
    //            }

    //            _timers[effect] = t;
    //        }
    //}
}
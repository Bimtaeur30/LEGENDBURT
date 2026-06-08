using System;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactContext
{
    // ── 공통 ──────────────────────────────────────────────
    public Player player;
    public GameObject target;

    // ── 전투 값 (Effect가 직접 수정) ──────────────────────
    public float damage;
    public bool isCritical;

    // ── 모듈 참조 (트리거 발생 모듈을 담아서 전달) ─────────
    private readonly Dictionary<Type, object> _modules = new();

    public void SetModule<T>(T module) where T : class
        => _modules[typeof(T)] = module;

    public T GetModule<T>() where T : class
        => _modules.TryGetValue(typeof(T), out var m) ? m as T : null;
}
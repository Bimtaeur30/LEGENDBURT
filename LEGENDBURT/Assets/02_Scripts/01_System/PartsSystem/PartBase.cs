using System;
using UnityEngine;

public abstract class PartBase : MonoBehaviour, IParts
{
    public event Action OnPartsDeactivate;
    public PartsDataSO PartsDataSO => partsDataSO;

    [SerializeField] private PartsDataSO partsDataSO;

    protected Player player;


    public virtual void Initialize(ModuleOwner owner)
    {
        player = owner as Player;
    }

    public abstract bool Activate();
    public virtual void Deactivate()
    {
        OnPartsDeactivate?.Invoke();
    }
    public abstract void DestroyParts();
}
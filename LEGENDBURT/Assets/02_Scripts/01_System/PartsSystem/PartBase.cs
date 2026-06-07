using UnityEngine;

public abstract class PartBase : MonoBehaviour, IParts
{
    [SerializeField] private PartsDataSO partsDataSO;
    public PartsDataSO PartsDataSO => partsDataSO;

    protected Player player;
    public virtual void Initialize(ModuleOwner owner)
    {
        player = owner as Player;
    }

    public abstract void Activate();
    public abstract void Deactivate();
    public abstract void DestroyParts();
}
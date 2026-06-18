using UnityEngine;

public interface IParts
{
    public PartsDataSO PartsDataSO { get; }
    bool Activate();
}

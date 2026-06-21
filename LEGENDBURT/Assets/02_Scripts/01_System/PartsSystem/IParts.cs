using System;
using UnityEngine;

public interface IParts
{
    public event Action OnPartsDeactivate;
    public PartsDataSO PartsDataSO { get; }
    bool Activate();
    void Deactivate();
    void DestroyParts();
}

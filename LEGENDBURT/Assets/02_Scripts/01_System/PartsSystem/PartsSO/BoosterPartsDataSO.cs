using UnityEngine;

[CreateAssetMenu(fileName = "PartsDataSO", menuName = "Library/PartsDataSO")]
public class BoosterPartsDataSO : PartsDataSO
{
    public ParticleSystem BoostParticlePrefab;
    public float BoostForce;
}

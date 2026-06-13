using UnityEngine;

[CreateAssetMenu(fileName = "BoosterPartsDataSO", menuName = "Library/PartsDataSO/BoosterPartsDataSO")]
public class BoosterPartsDataSO : PartsDataSO
{
    public ParticleSystem BoostParticlePrefab;
    public float BoostForce;
}

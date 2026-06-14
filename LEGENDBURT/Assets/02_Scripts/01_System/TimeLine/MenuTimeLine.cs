using UnityEngine;

public class MenuTimeLine : MonoBehaviour
{
    [SerializeField] private ParticleSystem FartParticle;

    public void Fart()
    {
        FartParticle.Play();
    }
}

using System.Collections;
using UnityEngine;

public class BoostItem : MonoBehaviour
{
    [SerializeField] private float respawnTime = 5f;
    [SerializeField] private GameObject visual; // 메시/이펙트 루트

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Player>(out var player)) return;

        player.BoostEffectModule.Activate();
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        visual.SetActive(false);
        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(respawnTime);

        visual.SetActive(true);
        GetComponent<Collider>().enabled = true;
    }
}
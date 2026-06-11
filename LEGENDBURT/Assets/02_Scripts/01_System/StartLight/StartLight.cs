using System;
using System.Collections;
using UnityEngine;

public class StartLight : MonoBehaviour
{
    [SerializeField] private GameObject[] lights;
    [SerializeField] private EventChannelSO playerChannel;

    private void Awake()
    {
        playerChannel.AddListener<AttachPartsEvent>(HandleAttachPartsEvent);
        foreach(var part in lights)
            part.SetActive(false);
    }

    private void HandleAttachPartsEvent(AttachPartsEvent @event)
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1f);// 처음 1초 대기

        for (int i = 0; i < lights.Length; i++)
        {
            GameObject light = lights[i];
            light.SetActive(true);
            yield return new WaitForSeconds(1f); // 1초마다 빛 하나씩 증가
        }

        foreach (var part in lights)
            part.SetActive(false);

        OnCountdownEnd();
    }

    private void OnCountdownEnd()
    {
        playerChannel.RasiseEvent(PlayerEvents.OnGameStartEvent);
        playerChannel.RasiseEvent(PlayerEvents.SetActivePlayerMovementInputEvent.Init(true));
    }
}

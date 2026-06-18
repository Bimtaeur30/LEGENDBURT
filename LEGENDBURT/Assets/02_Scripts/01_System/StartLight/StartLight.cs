using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class StartLight : MonoBehaviour
{
    [SerializeField] private GameObject[] lights;
    [SerializeField] private EventChannelSO playerChannel;
    [SerializeField] private TextMeshProUGUI chatTxt;
    [SerializeField] private Animator animator;
    [SerializeField] private EventChannelSO SoundChannel;
    [SerializeField] private SoundClipSO BeepSoundClip;
    [SerializeField] private SoundClipSO BeepSoundClip2;

    private void Awake()
    {
        playerChannel.AddListener<OnGameReadyEvent>(HandleOnGameReadyEvent);
        foreach(var part in lights)
            part.SetActive(false);
    }

    private void OnDestroy()
    {
        playerChannel.RemoveListener<OnGameReadyEvent>(HandleOnGameReadyEvent);
    }

    private void HandleOnGameReadyEvent(OnGameReadyEvent @event)
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
            chatTxt.text = (3 - i).ToString() + "...";
            SoundChannel.RasiseEvent(SoundEvents.PlaySoundEvent.Init(BeepSoundClip, transform, 0));
            yield return new WaitForSeconds(1f); // 1초마다 빛 하나씩 증가
        }

        SoundChannel.RasiseEvent(SoundEvents.PlaySoundEvent.Init(BeepSoundClip2, transform, 0));
        foreach (var part in lights)
            part.SetActive(false);

        chatTxt.text = "GO!!";
        OnCountdownEnd();
    }

    private void OnCountdownEnd()
    {
        animator.SetTrigger("FLY");
        playerChannel.RasiseEvent(PlayerEvents.OnGameStartEvent);
        playerChannel.RasiseEvent(PlayerEvents.SetActivePlayerMovementInputEvent.Init(true));
    }

    public void SetActivaeFalseThis()
    {
        gameObject.SetActive(false);
    }
}

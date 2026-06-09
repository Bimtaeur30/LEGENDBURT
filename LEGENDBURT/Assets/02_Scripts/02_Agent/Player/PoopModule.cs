using UnityEngine;

public class PoopModule : MonoBehaviour
{
    [Header("Event")]
    [SerializeField] private EventChannelSO playerChannel;
    [Header("Source")]
    [SerializeField] private Transform hipModel;
    [SerializeField] private SphereFillController sfc;
    [Header("Settings")]
    [SerializeField] private float poopTime = 60;
    [SerializeField] private float shakeInterval = 0.02f;
    [SerializeField] private float shakeAmount = 0.03f;

    private float curTime = 0;
    private bool runningGame = false;
    private Vector3 _originPos;
    private float _timer;

    private void Awake()
    {
        playerChannel.AddListener<OnGameStartEvent>(HandleOnGameStartEvent);
    }

    private void HandleOnGameStartEvent(OnGameStartEvent @event)
    {
        runningGame = true;
    }

    private void Start()
    {
        _originPos = hipModel.transform.localPosition;
    }
    private void Update()
    {
        if (!runningGame) return;

        curTime += Time.deltaTime;
        sfc.SetFillValue((curTime/poopTime) * 100);
        if (curTime > poopTime)
        {
            Debug.Log("게임 종료");
            playerChannel.RasiseEvent(PlayerEvents.OnGameOverEvent);
        }

        if (sfc.FillValue > 50)
        {
            _timer += Time.deltaTime;

            if (_timer >= shakeInterval)
            {
                _timer = 0f;
                Vector3 randomOffset = Random.insideUnitSphere * shakeAmount * (sfc.FillValue / 50);
                hipModel.transform.localPosition = _originPos + randomOffset;
            }
        }
    }
}

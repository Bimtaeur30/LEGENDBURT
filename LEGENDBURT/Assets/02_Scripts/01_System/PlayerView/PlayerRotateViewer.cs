using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerRotateViewer : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private EventChannelSO playerChannel;
    [SerializeField] private float rotateSpeed = 90f;

    private bool onGameEnd = false;
    private void Awake()
    {
        cinemachineCamera.Priority = 0;
        playerChannel.AddListener<OnGameOverEvent>(HandleOnGameOverEvent);
    }

    private void HandleOnGameOverEvent(OnGameOverEvent @event)
    {
        onGameEnd = true;
        cinemachineCamera.Priority = 100;
    }

    private void Update()
    {
        if (!onGameEnd) return;

        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }
}

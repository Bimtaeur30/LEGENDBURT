using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Tutorial_UI : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private EventChannelSO tutorialChannel;
    [Header("UI")]
    [SerializeField] private CanvasGroup tutorialCanvas;
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TextMeshProUGUI descriptionTxt;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Button closeBtn;

    private void Awake()
    {
        tutorialChannel.AddListener<ShowTutorialPageEvent>(HandleShowTutorialPageEvent);
        closeBtn.onClick.AddListener(HandleCloseBtnClick);
        HandleCloseBtnClick();
    }


    private void OnDestroy()
    {
        tutorialChannel.RemoveListener<ShowTutorialPageEvent>(HandleShowTutorialPageEvent);
    }

    private void HandleShowTutorialPageEvent(ShowTutorialPageEvent @event)
    {
        Time.timeScale = 0;
        tutorialCanvas.DOFade(1f, 1f).SetUpdate(true);
        tutorialCanvas.interactable = true;
        tutorialCanvas.blocksRaycasts = true;

        titleTxt.text = @event.TutorialData.TutorialTitle;
        descriptionTxt.text = @event.TutorialData.TutorialDescription;
        videoPlayer.clip = @event.TutorialData.TutorialVideo;
        videoPlayer.Play();
    }
    private void HandleCloseBtnClick()
    {
        Time.timeScale = 1;
        tutorialCanvas.DOFade(0f, 1f).SetUpdate(true);
        tutorialCanvas.interactable = false;
        tutorialCanvas.blocksRaycasts = false;
    }
}

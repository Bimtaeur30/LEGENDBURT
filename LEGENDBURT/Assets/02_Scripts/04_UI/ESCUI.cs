using Assets._02_Scripts._01_System.Stage;
using Coffee.UIEffects;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ESCUI : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private EventChannelSO stageChannel;
    [SerializeField] private EventChannelSO soundChannel;
    [SerializeField] private SoundClipSO swooshClip;

    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private SettingsUI settingUI;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private Ease moveEase = Ease.OutCubic;

    [Header("Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button quitButton;

    private bool isOpen;

    private float leftOffscreenX;
    private float rightOffscreenX;

    private void Awake()
    {
        RectTransform canvasRect =
            canvasGroup.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        float canvasWidth = canvasRect.rect.width;
        float panelWidth = panelRect.rect.width;

        leftOffscreenX = -(canvasWidth * 0.5f + panelWidth * 0.5f);
        rightOffscreenX = canvasWidth * 0.5f + panelWidth * 0.5f;

        panelRect.anchoredPosition =
            new Vector2(rightOffscreenX, panelRect.anchoredPosition.y);

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        backButton.onClick.AddListener(CloseESC);
        settingButton.onClick.AddListener(OpenSetting);
        menuButton.onClick.AddListener(GoToMenu);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isOpen)
            {
                CloseESC();
            }
            else
            {
                OpenESC();
            }
        }
    }

    private void OpenESC()
    {
        isOpen = true;

        canvasGroup.DOKill();
        panelRect.DOKill();

        canvasGroup.DOFade(1f, fadeDuration).SetUpdate(true);

        panelRect.DOAnchorPosX(0f, moveDuration)
            .SetEase(moveEase).SetUpdate(true);

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        soundChannel.RasiseEvent(SoundEvents.PlaySoundEvent.Init(swooshClip));

        Time.timeScale = 0f;
    }

    private void OpenSetting()
    {
        settingUI.Open();
    }

    private void CloseESC()
    {
        isOpen = false;

        canvasGroup.DOKill();
        panelRect.DOKill();

        canvasGroup.DOFade(0f, fadeDuration).SetUpdate(true);

        panelRect.DOAnchorPosX(leftOffscreenX, moveDuration)
            .SetEase(moveEase).SetUpdate(true)
            .OnComplete(() =>
            {
                panelRect.anchoredPosition = new Vector2(
                    rightOffscreenX,
                    panelRect.anchoredPosition.y);
            });

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        soundChannel.RasiseEvent(SoundEvents.PlaySoundEvent.Init(swooshClip));

        Time.timeScale = 1f;
    }

    private void GoToMenu()
    {
        Time.timeScale = 1f;
        stageChannel.RasiseEvent(StageEvents.RemoveStageSaveDataEvent);
    }

    private void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
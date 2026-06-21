using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingsUI : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Buttons")]
    [SerializeField] private Button closeButton;

    [Header("Audio")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Screen")]
    [SerializeField] private TMP_Dropdown screenModeDropdown;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 0.25f;

    private Tween _fadeTween;

    private const string SCREEN_MODE_KEY = "SCREEN_MODE";

    private void Awake()
    {
        InitializeAudio();
        InitializeScreenMode();

        closeButton.onClick.AddListener(Close);

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    private void OnDestroy()
    {
        closeButton.onClick.RemoveListener(Close);

        masterSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
        bgmSlider.onValueChanged.RemoveListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);

        screenModeDropdown.onValueChanged.RemoveListener(OnScreenModeChanged);
    }

    #region Initialize

    private void InitializeAudio()
    {
        masterSlider.minValue = 0;
        masterSlider.maxValue = 100;

        bgmSlider.minValue = 0;
        bgmSlider.maxValue = 100;

        sfxSlider.minValue = 0;
        sfxSlider.maxValue = 100;

        masterSlider.SetValueWithoutNotify(SoundManager.Instance.MasterVolume);
        bgmSlider.SetValueWithoutNotify(SoundManager.Instance.BGMVolume);
        sfxSlider.SetValueWithoutNotify(SoundManager.Instance.SFXVolume);

        masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void InitializeScreenMode()
    {
        screenModeDropdown.ClearOptions();

        screenModeDropdown.AddOptions(new List<string>
        {
            "전체화면",
            "창모드"
        });

        int mode = PlayerPrefs.GetInt(SCREEN_MODE_KEY, 0);

        screenModeDropdown.SetValueWithoutNotify(mode);
        ApplyScreenMode(mode);

        screenModeDropdown.onValueChanged.AddListener(OnScreenModeChanged);
    }

    #endregion

    #region Open Close

    public void Open()
    {
        _fadeTween?.Kill();

        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        _fadeTween = canvasGroup
            .DOFade(1f, fadeDuration)
            .SetUpdate(true);
    }

    public void Close()
    {
        _fadeTween?.Kill();

        _fadeTween = canvasGroup
            .DOFade(0f, fadeDuration)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;

                PlayerPrefs.Save();
            });
    }

    #endregion

    #region Audio

    private void OnMasterVolumeChanged(float value)
    {
        SoundManager.Instance.SetMasterVolume(value);
    }

    private void OnBGMVolumeChanged(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
    }

    #endregion

    #region Screen

    private void OnScreenModeChanged(int index)
    {
        ApplyScreenMode(index);

        PlayerPrefs.SetInt(SCREEN_MODE_KEY, index);
    }

    private void ApplyScreenMode(int index)
    {
        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;

            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    #endregion
}
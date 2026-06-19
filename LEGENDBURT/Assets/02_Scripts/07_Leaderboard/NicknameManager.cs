using Assets._02_Scripts._01_System.Stage;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NicknameManager : MonoSingleton<NicknameManager>
{
    public bool IsNickNameChoosing { get; private set; } = false;

    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private GameObject nickNamePanel;
    [SerializeField]
    private EventChannelSO stageChannel;

    private const string NicknameKey = "Nickname";

    private void Start()
    {
        if (PlayerPrefs.HasKey(NicknameKey))
        {
            nickNamePanel.gameObject.SetActive(false);
        }
        else
        {
            nickNamePanel.gameObject.SetActive(true);
            IsNickNameChoosing = true;
        }
    }

    public async void RegisterNickname()
    {
        if (PlayerPrefs.HasKey(NicknameKey))
            return;

        string nickname = inputField.text;

        await AuthenticationService.Instance
            .UpdatePlayerNameAsync(nickname);

        PlayerPrefs.SetString(
            NicknameKey,
            nickname);

        PlayerPrefs.Save();

        nickNamePanel.gameObject.SetActive(false);
        IsNickNameChoosing = false;

        stageChannel.RasiseEvent(StageEvents.LoadTutorialEvent);
    }

    [ContextMenu("Delete Nickname Pref")]
    public void DeleteNicknamePref()
    {
        PlayerPrefs.DeleteKey(NicknameKey);
        PlayerPrefs.Save();

        Debug.Log("Nickname Pref Deleted");
    }
}
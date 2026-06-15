using TMPro;
using Unity.Services.Authentication;
using UnityEngine;

public class NicknameManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private GameObject nickNamePanel;

    private const string NicknameKey = "Nickname";

    private void Start()
    {
        if (PlayerPrefs.HasKey(NicknameKey))
        {
            nickNamePanel.gameObject.SetActive(false);
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
    }
}
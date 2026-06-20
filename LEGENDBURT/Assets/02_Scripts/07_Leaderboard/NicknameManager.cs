using Unity.Services.Authentication;
using UnityEngine;

public class NicknameManager : MonoSingleton<NicknameManager>
{
    private const string NicknameKey = "Nickname";

    public string Nickname { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Nickname = PlayerPrefs.GetString(NicknameKey, string.Empty);
    }

    public bool HasNickname()
    {
        return !string.IsNullOrEmpty(Nickname);
    }

    public async System.Threading.Tasks.Task<bool> RegisterNicknameAsync(string nickname)
    {
        nickname = nickname.Trim();

        if (string.IsNullOrEmpty(nickname))
            return false;

        await AuthenticationService.Instance
            .UpdatePlayerNameAsync(nickname);

        PlayerPrefs.SetString(NicknameKey, nickname);
        PlayerPrefs.Save();

        Nickname = nickname;

        return true;
    }

    public void DeleteNickname()
    {
        PlayerPrefs.DeleteKey(NicknameKey);
        PlayerPrefs.Save();

        Nickname = string.Empty;
    }

    [ContextMenu("Delete Nickname Pref")]
    private void DeleteNicknamePrefContextMenu()
    {
        DeleteNickname();
        Debug.Log("Nickname Pref Deleted");
    }
}
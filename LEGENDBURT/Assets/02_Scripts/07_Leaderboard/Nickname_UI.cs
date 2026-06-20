using Assets._02_Scripts._01_System.Stage;
using TMPro;
using UnityEngine;

public class Nickname_UI : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject nicknamePanel;
    [SerializeField] private EventChannelSO stageChannel;

    public bool IsNickNameChoosing { get; private set; }

    private void Start()
    {
        bool hasNickname = NicknameManager.Instance.HasNickname();

        nicknamePanel.SetActive(!hasNickname);
        IsNickNameChoosing = !hasNickname;
    }

    public async void RegisterNickname()
    {
        bool success =
            await NicknameManager.Instance.RegisterNicknameAsync(inputField.text);

        if (!success)
            return;

        nicknamePanel.SetActive(false);
        IsNickNameChoosing = false;

        stageChannel.RasiseEvent(StageEvents.LoadTutorialEvent);
    }
}
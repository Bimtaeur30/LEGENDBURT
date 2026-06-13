using UnityEngine;

public class ChatModule : MonoBehaviour, IModule
{
    [SerializeField] private RectTransform chatParent;
    [SerializeField] private BubbleChat_UI charPrefab;
    public void Initialize(ModuleOwner owner) { }

    public void GenerateChat(string message)
    {
        for (int i = 0; i < chatParent.childCount; i++)
        {
            BubbleChat_UI oldChat = chatParent.GetChild(i).GetComponent<BubbleChat_UI>();
            oldChat.DestroyChat();

        }

        BubbleChat_UI chat = Instantiate(charPrefab, chatParent);
        chat.Initialize(message);
    }
}
